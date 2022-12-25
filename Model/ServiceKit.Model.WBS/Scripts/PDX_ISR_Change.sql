SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[PDX_ISR_Change]
	@Project varchar(1024),
	@Code varchar(64),
	@Descr varchar(512),
	@FD tinyint = 0,
	@KeyField int,
	@ParentKeyField int,
	@Actual varchar(3) = null,
	@NC varchar(128),
	@Act int, --1--ceate, 2-update,3 delete
	@Chk bit = 0
	
as
	declare  @Res int

	declare @ErrList varchar(1024), @blnInAct bit

	select @Res = 0,@ErrList = ''
	
	set @Actual = ISNULL(@Actual,'')
	select
		@Project = trim(isnull(@Project,'')),
		@Descr = trim(isnull(@Descr,'')),
		@NC  = trim(isnull(@NC,'')),
		@FD  = isnull(@FD,0)
	select @Res = @@error if @Res <> 0 return @Res
	
	set @blnInAct = case @Actual when 'ДА' then 0 else 1 end
	
    if @ParentKeyField = 0
    begin
		set @ParentKeyField = null
	end
	else
	begin
		set @Project = ''
    end
    if @KeyField = 0
    begin
		set @KeyField = null
    end
    
/************************************
ПРОВЕРКИ ВВЕДЕННЫХ ЗНАЧЕНИЙ
*************************************/
if @Act in (1,2)
begin

	If @Descr = ''
	begin
		select @ErrList = @ErrList + case len(@ErrList) when 0 then '' else '<br>' end +
			'Укажите наименование'
		return -1
	end

	If @Project <> ''
	begin
		If not exists (select 1 from dbo.UserDirValues where UDKeyField = 35 and Field1 = @Project)
		begin
			select @ErrList = @ErrList + case len(@ErrList) when 0 then '' else '<br>' end +
				'Указанный Проект не существует'
			select @Res = @@error if @Res <> 0 return @Res
		end
	end

	If exists (select 1 from dbo.UserDir_ISR where Code = @Code and KeyField <> isnull(@KeyField,-1))
	begin
			select @ErrList = @ErrList + case len(@ErrList) when 0 then '' else '<br>' end +
				'Указанный Код ИСР уже существует'
			select @Res = @@error if @Res <> 0 return @Res
	end
end

If @Act = 3
begin
  if exists (select 1 from dbo.UserDir_ISR where ParentKeyField = @KeyField)
  begin
		select @ErrList = @ErrList + case len(@ErrList) when 0 then '' else '<br>' end +
			'Нельзя удалить данный узел, у него есть подчиненные'
		select @Res = @@error if @Res <> 0 return @Res
  end  
end

/************************************/

If @Chk = 0
begin -- если это создание, то нужно генерить ошибку, если она есть
	If @ErrList <> '' 
	begin
		RAISERROR (@ErrList, 16,1)
		return @Res
	end
end
else --если это проверка, просто выводим сообщение даже если оно пустое
begin
	select @ErrList as ErrList 
	select @Res = @@error if @Res <> 0 return @Res
end

IF @ErrList = '' and @Chk = 0
BEGIN
	if @Act = 2
	begin
		update dbo.UserDir_ISR
			set 
				Code = @Code,
				Project = @Project,
				Descr = @Descr,
				FD = @FD,
				ParentKeyField = @ParentKeyField,
				inActive = @blnInAct,
				NLM = @NC,
				DLM = getdate()
		where KeyField = @KeyField
		select @Res = @@error if @Res <> 0 return @Res
	end

	If @Act = 1
	begin
		insert into dbo.UserDir_ISR
			(Code, Project, Descr, FD, ParentKeyField, inActive, NC, DC, NLM, DLM)
		values 
			(@Code, @Project, @Descr, @FD, @ParentKeyField, @blnInAct, @NC, getdate(), @NC, getdate())
		select @Res = @@error if @Res <> 0 return @Res
	end

	If @Act = 3
	begin
		delete from dbo.UserDir_ISR where KeyField = @KeyField
		select @Res = @@error if @Res <> 0 return @Res
	end
END
	select @Res = @@error if @Res <> 0 return @Res

	return @Res



