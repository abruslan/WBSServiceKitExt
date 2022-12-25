SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[PDX_ISR_Synchronize] 
	@RequestId uniqueidentifier,  -- идентификатор сессии
	@DeleteNotFound bit = 0, -- если 1, то удаляем все ИСР по данному проекту, которых нет в таблице WBS_SyncRequestItems для данной сессии. Этот режим по сути означает полную перезаливку содержания ИСР по проекту
	@Chk bit = 0,  -- режим проверки (1) либо изменения (0)
	@NC varchar(128)  -- логин или имя автора изменений. возможно, правильнее 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare  @Res int
declare @ErrList varchar(1024)

declare @ProjectName varchar(1024)
declare @OldName varchar(512)

declare @ErrCounterAdd int
declare @ErrCounterEdit int
declare @ErrCounterDelete int

select @Res = 0
select @ErrList = ''
select @ProjectName = ''
select @ErrCounterAdd = 0
select @ErrCounterEdit = 0
select @ErrCounterDelete = 0

-- блок проверок исходных данных
begin 
	--если не нашли такую сессию, то возвращаем ошибку
	If not exists (select 1 from dbo.WBS_SyncRequests where RequestId = @RequestId) 
	begin
		select @Res = -1
		Select @ErrList = 'Не найдена сессия с идентификатором ' + convert(varchar(512), @RequestId)
		RAISERROR (@ErrList, 16,1)
		return @Res
	end

	--если пустое название проекта, то возвращаем ошибку
	select @ProjectName = ProjectName from dbo.WBS_SyncRequests where RequestId = @RequestId
	if isnull(@ProjectName, '') = ''
	begin
		select @Res = -1
		Select @ErrList = 'Указано пустое название проекта для сессии с идентификатором ' + convert(varchar(512), @RequestId)
		RAISERROR (@ErrList, 16,1)
		return @Res
	end
end
-- конец блока проверок

declare @CurrentID uniqueidentifier
declare @ParentID uniqueidentifier
declare @Code varchar(64)
declare @ParentCode varchar(64)
declare @ShortName varchar(512)
declare @FD tinyint
declare @KeyField int
declare @ParentKeyField int
declare @Actual varchar(3)
declare @Act int

select @Actual = 'ДА'
select @FD = 0

-- добавляем проект, если его еще нет
	select @Code = ProjectCode from dbo.WBS_SyncRequests where RequestId = @RequestId

	If not exists (select * from UserDir_ISR where Code = isnull(@Code, 0)) 
	begin
		select @Act = 1
        exec @Res = [dbo].[PDX_ISR_Change] @ProjectName, @Code, @ProjectName, @FD, 0, NULL, @Actual, @NC, @Act, @Chk

		insert into UserDirValues( UDKeyField, Field1, Field2, Field3, Field4, Field5, Field6, NameLastModification, DateLastModification)
		values                   (35, @ProjectName, @Code, '', '', 1, '', @NC, GETDATE())  

		-- всегда добавляем в новый проект один элемент - "общая часть" с кодом _10. добавляем в исходную таблицу, чтобы механизм сам добавил этот элемент в справочник ИСР
		--select @ParentKeyField = KeyField from dbo.UserDir_ISR where Code = @Code
		select @Code = @Code + '_10'
		select @ShortName = 'Общая часть'
		if not exists (select * from UserDir_ISR where Code = isnull(@Code, 0))
		begin
		if not exists (select * from WBS_SyncRequestItems where RequestId = @RequestId and FullCode = @Code)
			begin 
		--exec @Res = [dbo].[PDX_ISR_Change] @ProjectName, @Code, @ShortName, @FD, @KeyField, @ParentKeyField, @Actual, @NC, @Act, @Chk
				insert into WBS_SyncRequestItems (RequestId, Id, ParentId, Level, ShortCode, FullCode, ShortName, FullName, 
				Comment, Status, ErrorMessage, Created)
				values (@RequestId, NEWID(), '00000000-0000-0000-0000-000000000000', 1, '10', @Code, @ShortName, @ShortName, 
				'В справочник ИСР обавлен обьект "Общая часть с кодом _10"', 1, null, GETDATE())
			end
		end
	end

-- проходим каждую запись требуемой сессии несколько раз. первый проход - добавление записей, второй - редактирование, третий - удаление.
-- результаты добавления и редактирования пишем в таблицу WBS_SyncRequestItems по каждой строке отдельно, в таблицу WBS_SyncRequests пишем общий результат - были ли ошибки при редактировании и добавлении.
-- если возникли ошибки при удалении ИСР, то заносим по каждой строке ИСР новую запись в таблицу WBS_SyncRequestItems и пишем результат туда. в таблицу WBS_SyncRequests пишем общий результат


 -- первый проход - редактируем существующие записи
 -- если новое название объекта совпадает с существующим, то ничего не меняем и так и пишем в лог
select @Act = 2
--объявляем курсор для выборки тех записей, которые нужно изменить
 DECLARE my_curEdit CURSOR FOR 
	select distinct I.FullCode, I.ShortName, ISR.KeyField, I.ParentId, I.Id, ISR.Descr
	from dbo.WBS_SyncRequestItems as I
	inner join UserDir_ISR as ISR on I.FullCode = ISR.Code
	where I.RequestId = @RequestId

   --открываем курсор
   OPEN my_curEdit

   --считываем данные первой строки в наши переменные
   FETCH NEXT FROM my_curEdit INTO @Code, @ShortName, @KeyField, @ParentID, @CurrentID, @OldName
   --если данные в курсоре есть, то заходим в цикл
   --и крутимся там до тех пор, пока не закончатся строки в курсоре
   WHILE @@FETCH_STATUS = 0
   BEGIN
      begin
        --на каждую итерацию цикла запускаем нашу основную процедуру с нужными параметрами   
		if len(@Code) > 6
		begin
			select @ParentCode = Fullcode from WBS_SyncRequestItems where ID = @ParentID 
		end
		else
		begin
			select @ParentCode = left(@Code,3)
		end
		select @ParentKeyField = KeyField from dbo.UserDir_ISR where Code = @ParentCode

		if @Chk = 0
		if exists(select * from UserDir_ISR where Code = @Code and Descr = @ShortName and inActive = 0)
		begin
			update WBS_SyncRequestItems set ErrorMessage = '', Comment = 'Новое название совпадает со старым. Запись оставлена без изменений в справочнике ИСР', Status = 1 where ID = @CurrentID
		end
		else
		begin
	        exec @Res = [dbo].[PDX_ISR_Change] @ProjectName, @Code, @ShortName, @FD, @KeyField, @ParentKeyField, @Actual, @NC, @Act, @Chk
			if @Res = 0
			begin
				if @ShortName = @OldName
				begin
					update WBS_SyncRequestItems set ErrorMessage = '', Comment = 'Запись помечена активной в справочнике ИСР.', Status = 1 where ID = @CurrentID
				end
				else
				begin
					update WBS_SyncRequestItems set ErrorMessage = '', Comment = 'Запись изменена в справочнике ИСР. Старое название - ' + @OldName, Status = 1 where ID = @CurrentID
				end
			end
			else
			begin
				update WBS_SyncRequestItems set Comment = '', ErrorMessage = 'Ошибка при редактировании записи в справочнике ИСР: ' + convert(varchar, @Res), Status = -1 where ID = @CurrentID
				select @ErrCounterEdit = @ErrCounterEdit + 1
			end
		end
	end	 
        --считываем следующую строку курсора
        FETCH NEXT FROM my_curEdit INTO @Code, @ShortName, @KeyField, @ParentID, @CurrentID, @OldName
   END

   
   --закрываем курсор
   CLOSE my_curEdit
   DEALLOCATE my_curEdit

-- второй проход - добавляем новые записи
select @Act = 1
--объявляем курсор для выборки тех записей, которых нет в таблице ИСР
 DECLARE my_curAdd CURSOR FOR 
	select distinct I.FullCode, I.ShortName, ISR.KeyField, I.ParentId, I.Id
	from dbo.WBS_SyncRequestItems as I
	left join UserDir_ISR as ISR on I.FullCode = ISR.Code
	where I.FullCode <> isnull(ISR.Code, 0)
	and I.RequestId = @RequestId ORDER BY I.FullCode ASC

   --открываем курсор
   OPEN my_curAdd

   --считываем данные первой строки в наши переменные
   FETCH NEXT FROM my_curAdd INTO @Code, @ShortName, @KeyField, @ParentID, @CurrentID
   --если данные в курсоре есть, то заходим в цикл
   --и крутимся там до тех пор, пока не закончатся строки в курсоре
   WHILE @@FETCH_STATUS = 0
   BEGIN

   begin
        --на каждую итерацию цикла запускаем нашу основную процедуру с нужными параметрами   
		if len(@Code) > 6
		begin
			select @ParentCode = Fullcode from WBS_SyncRequestItems where ID = @ParentID 
		end
		else
		begin
			select @ParentCode = left(@Code,3)
		end
		select @ParentKeyField = KeyField from dbo.UserDir_ISR where Code = @ParentCode

        exec @Res = [dbo].[PDX_ISR_Change] @ProjectName, @Code, @ShortName, @FD, @KeyField, @ParentKeyField, @Actual, @NC, @Act, @Chk
		if @Chk = 0
		begin
			if @Res = 0
			begin
				update WBS_SyncRequestItems set ErrorMessage = '', Comment = 'Запись добавлена в справочник ИСР', Status = 1 where ID = @CurrentID
			end
			else
			begin
				update WBS_SyncRequestItems set Comment = '', ErrorMessage = 'Ошибка при добавлении записи в справочник ИСР: ' + convert(varchar, @Res), Status = -1 where ID = @CurrentID
				select @ErrCounterAdd = @ErrCounterAdd + 1
			end
		end
	end	 
        --считываем следующую строку курсора
        FETCH NEXT FROM my_curAdd INTO @Code, @ShortName, @KeyField, @ParentID, @CurrentID
   END

   
   --закрываем курсор
   CLOSE my_curAdd
   DEALLOCATE my_curAdd


   -- третий проход - редактируем существующие записи. Пока реализован только вариант "Удаляем все по данному проекту, чего нет в табличке WBS_SyncRequestItems для текущей сессии. 
   -- для того, чтобы удалять поштучно на основе того, что прописано в исходном екселе, надо добавить в табличку WBS_SyncRequestItems поле "что сделать", туда помещать отметку о необходимости удаления, и тогда просто нужно будет открыть курсор с выборкой по данной сессии и присутсивю пометки на удаление, пройтись по нему и поштучно удалить
select @Act = 3
--объявляем курсор для выборки тех записей, которые нужно удалить. не нравится то, что здесь мы выбираем по коду проекта, но других вариантов не видно 
 DECLARE my_curDelete CURSOR FOR 
	select distinct  ISR.Code, ISR.Descr, ISR.KeyField, I.ParentId, I.Id
	from (select * from dbo.WBS_SyncRequestItems where RequestId = @RequestId) as I
	right join UserDir_ISR as ISR on I.FullCode = ISR.Code
	where Left(ISR.Code, 3) = (select ProjectCode from WBS_SyncRequests where RequestId = @RequestId)
	and len(ISR.Code) <> 3 
	and I.FullCode is null ORDER BY ISR.Code DESC


   --открываем курсор
   OPEN my_curDelete

   --считываем данные первой строки в наши переменные
   FETCH NEXT FROM my_curDelete INTO @Code, @ShortName, @KeyField, @ParentID, @CurrentID
   --если данные в курсоре есть, то заходим в цикл
   --и крутимся там до тех пор, пока не закончатся строки в курсоре
   WHILE @@FETCH_STATUS = 0
   BEGIN
		if @Chk = 0
		begin
			if exists (select * from UserDir_ISR where Code = @Code and inActive = 0) 
			begin
				update UserDir_ISR set inActive = 1 where Code = @Code 
	--		update WBS_SyncRequestItems set Comment = 'Запись удалена в справочнике ИСР', Status = 1 where ID = @CurrentID
				insert into WBS_SyncRequestItems (RequestId, Id, ParentId, Level, ShortCode, FullCode, ShortName, FullName, 
				Comment, Status, ErrorMessage, Created)
				values (@RequestId, NEWID(), null, 0, right(@Code, 2), @Code, @ShortName, @ShortName, 
				'В справочнике ИСР помечена неактивной запись с кодом ' + convert(varchar(20), @Code) + 'значение поля KeyField = ' + convert(varchar(20), @KeyField),
				1, null, GETDATE())
			end
		end
        --считываем следующую строку курсора
        FETCH NEXT FROM my_curDelete INTO @Code, @ShortName, @KeyField, @ParentID, @CurrentID
	END
   
	--закрываем курсор
	CLOSE my_curDelete
	DEALLOCATE my_curDelete
-- если при проходе нашли элементы, которые нельзя было удалить (из-за наличия подчиненных), то нужно повторить проход после удаления подчиненных


	select @Res = @ErrCounterAdd + @ErrCounterEdit + @ErrCounterDelete
	if @Res = 0 
	begin
		update WBS_SyncRequests set Status = 1, ErrorMessage = '' where RequestId = @RequestId
	end
	else
	begin
		update WBS_SyncRequests set Status = -1, ErrorMessage = 'При добавлении записей в справочник ИСР возникло ' + CONVERT(varchar(10), @ErrCounterAdd) + ' ошибок, при редактировании ' + CONVERT(varchar(10), @ErrCounterEdit) + ', при удалении ' + CONVERT(varchar(10), @ErrCounterDelete) where RequestId = @RequestId
	end

	select @Res = @ErrCounterAdd + @ErrCounterEdit + @ErrCounterDelete
	return @Res

END
