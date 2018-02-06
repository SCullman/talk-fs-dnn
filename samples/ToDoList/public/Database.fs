module DnnSummit.ToDo.Logic

open SharedTypes
open PetaPoco


[<TableName "SummitTasks">]
[<PrimaryKey "Id">]
type ToDoItemInfo () = 
  member val Id = System.Guid.NewGuid() with get, set
  member val ModuleId = 0 with get,set
  member val Task = "" with get,set
  member val Complete = false with get,set
  member val CreatedByUserId = 0 with get,set
  member val CreatedOnDate = System.DateTime.Now with get,set
  member val LastModifiedByUserId = 0 with get,set
  member val LastModifiedOnDate = System.DateTime.Now with get,set

let add (item:ToDoItem) moduleId userId =
    let info = ToDoItemInfo ()
    info.Id <- item.Id
    info.ModuleId <- moduleId
    info.Task <- item.Task
    info.Complete <- item.Complete
    info.CreatedByUserId <- userId
    info.LastModifiedByUserId <- userId

    use db = new Database "SiteSqlServer"
    db.Insert ("SummitTasks", "Id", false, info) |> ignore

let complete (id:Id) userId =
    use db = new Database "SiteSqlServer"
    let info = db.Single<ToDoItemInfo> ("WHERE Id = @0", id)
    info.Complete <- true
    info.LastModifiedByUserId <- userId
    info.LastModifiedOnDate <- System.DateTime.Now
    db.Update info |> ignore

let delete (id:Id) = 
    use db = new Database "SiteSqlServer"
    db.Delete<ToDoItemInfo> ("WHERE Id = @0", id) |> ignore

let getList (moduleId:int) = 
   use db = new Database "SiteSqlServer"
   db.Query<ToDoItemInfo>("WHERE moduleId = @0", moduleId)
   |> Seq.map(fun i -> {Id = i.Id; Task = i.Task; Complete = i.Complete})
   |> Seq.toList


