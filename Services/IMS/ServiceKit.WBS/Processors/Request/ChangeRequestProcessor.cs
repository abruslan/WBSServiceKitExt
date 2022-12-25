using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors.Request
{
    public class ChangeRequestProcessor : RequestProcessor, IRequestProcessor
    {
        public ChangeRequestProcessor(AppDbContext context) : base(context)
        {
        }

        public WBS_Project Eval(WBS_Request request)
        {
            var dtStarted = DateTime.Now;
            var project = _context.WBS_Projects
                .Include(r => r.ProjectItems).Where(r => r.ProjectCode == request.ProjectCode && !r.IsDeleted)
                .FirstOrDefault();
            if (project == null)
            {
                ErrorMessage.Add("Не найден проект для обновления. Необходимо предварительно загрузить проект в систему.");
                return null;
            }

            if (!CheckCommand(request, project))
            {
                ErrorMessage.AddRange(request.Changes.Where(r => !string.IsNullOrEmpty(r.ErrorMessage)).Select(r => r.ErrorMessage).ToList());
                return null;
            }

            if (!CheckLength(request, project))
                return null;

            foreach (var item in project.ProjectItems.Where(r => r.Created >= dtStarted))
                _context.Entry(item).State = EntityState.Added;

            // Сохранили в лог текущую версию
            //SaveLog(project);

            _context.SaveChanges();
            return project;
        }

        private void SaveLog(WBS_Project project)
        {
            throw new NotImplementedException();
        }

        public bool Check(WBS_Request request)
        {
            // Важно! Данные проекта не меняем, используем только для чтения
            var project = _context.WBS_Projects
                .Include(r => r.ProjectItems).Where(r => r.ProjectCode == request.ProjectCode && !r.IsDeleted)
                .AsNoTracking()
                .FirstOrDefault();

            if (project == null)
            {
                ErrorMessage.Add("Не найден проект для обновления. Необходимо предварительно загрузить проект в систему.");
                return false;
            }
            if (string.IsNullOrEmpty(project.ProjectShortName))
            {
                ErrorMessage.Add("Не заполнено короткое название проекта, необходимое для проверки длины наименования в 1С.");
                return false;
            }

            foreach (var item in request.Changes)
                item.ErrorMessage = "";

            if (!CheckCommand(request, project))
            {
                ErrorMessage.AddRange(request.Changes.Where(r => !string.IsNullOrEmpty(r.ErrorMessage)).Select(r => r.ErrorMessage).ToList());
                _context.RollBack();
                return false;
            }

            if (!CheckLength(request, project))
            {
                _context.RollBack();
                return false;
            }

            _context.RollBack();
            return true;
        }

        private bool CheckLength(WBS_Request request, WBS_Project project)
        {
            foreach (var item in project.ProjectItems.Where(r => r.Level == 1 && !r.IsDeleted))
            {
                item.Full1CName = $"{project.ProjectShortName}_{item.ShortName}";
                var requestItem = request.Changes.FirstOrDefault(r => item.FullCode == $"{request.ProjectCode}_{r.Code}");
                if (requestItem != null)
                    requestItem.Full1CName = item.Full1CName;
                CheckLenghtItem(request, project, item, null);
            }
            return ErrorMessage.Count == 0; // request.Changes.Any(r => !string.IsNullOrEmpty(r.ErrorMessage));
        }

        private void CheckLenghtItem(WBS_Request request, WBS_Project project, WBS_ProjectItem item, WBS_ProjectItem parent)
        {
            item.Full1CName = parent == null ? $"{project.ProjectShortName}_{item.ShortName}" : $"{parent.Full1CName}_{item.ShortName}";
            if (item.Full1CName.Length > FULL_NAME_MAX_LENGTH)
            {
                ErrorMessage.Add($"Длина полного наименования для элемента {item.FullCode} составляет {item.Full1CName.Length} символов: '{item.Full1CName}'.");
            }
            foreach(var child in project.ProjectItems.Where(r => r.ParentId == item.Id && !r.IsDeleted))
            {
                CheckLenghtItem(request, project, child, item);
            }
        }

        private bool CheckCommand(WBS_Request request, WBS_Project project)
        {
            foreach (var item in request.Changes.Where(r => !r.IsDeleted).OrderBy(r => r.NewCode))
            {
                var shortCode = string.IsNullOrEmpty(item.NewCode?.Trim()) ? item.Code : item.NewCode;
                var fullCode = $"{project.ProjectCode}_{shortCode}";
                switch ((item.Comment??"").ToLower())
                {
                    case "создать":
                    case "создание":
                    case "создание объекта":
                        if (project.ProjectItems.Any(r => r.FullCode == fullCode))
                        {
                            item.ErrorMessage = $"В проекте уже заведен код {fullCode}";
                            return false;
                        }
                        WBS_ProjectItem parent = null;
                        if (shortCode?.Length > 2)
                        {
                            var parentCode = fullCode.Substring(0, fullCode.Length - 2);
                            parent = project.ProjectItems.Where(r => r.FullCode == parentCode).FirstOrDefault();
                            if (parent == null)
                            {
                                item.ErrorMessage = $"В проекте не найден родительский элемент с кодом {parentCode}";
                                return false;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(item.NewName))
                        {
                            item.ErrorMessage = $"Не указано название для элемента с кодом {fullCode}";
                            return false;
                        }
                        WBS_ProjectItem current = new WBS_ProjectItem() {
                            Id = Guid.NewGuid(),
                            Project = project,
                            ParentId = parent?.Id,
                            Level = (parent?.Level??0) + 1,
                            ShortCode = shortCode,
                            FullCode = fullCode,
                            ShortName = item.NewName,
                            Comment = item.Comment,
                            Created = DateTime.Now
                        };
                        project.ProjectItems.Add(current);
                        break;
                    case "переименование":
                    case "переименовать":
                    case "изменение наименования":
                        var itemUpdating = project.ProjectItems.Where(r => r.FullCode == fullCode).FirstOrDefault();
                        if (itemUpdating == null)
                        {
                            item.ErrorMessage = $"В проекте не найден элемент с кодом {fullCode}";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(item.NewName))
                        {
                            item.ErrorMessage = $"Не указано название для элемента с кодом {fullCode}";
                            return false;
                        }
                        itemUpdating.ShortName = item.NewName;
                        itemUpdating.Comment = item.Comment;
                        break;
                    case "удалить":
                    case "удаление":
                    case "удаление объекта":
                        var itemDeleting = project.ProjectItems.Where(r => r.FullCode == fullCode).FirstOrDefault();
                        if (itemDeleting == null)
                        {
                            item.ErrorMessage = $"В проекте не найден элемент с кодом {fullCode}";
                            return false;
                        }
                        DeleteProjectItem(project, itemDeleting);
                        break;
                    default:
                        item.ErrorMessage = $"Неопределенный тип изменения '{item.Comment}' (старый код {item.Code}, новый код {item.NewCode})";
                        return false;
                }
            }
            return true;
        }

        private void DeleteProjectItem(WBS_Project project, WBS_ProjectItem itemDeleting)
        {
            _context.RemoveRange(project.ProjectItems.Where(r => r.FullCode.StartsWith(itemDeleting.FullCode)).ToList());
        }
    }
}
