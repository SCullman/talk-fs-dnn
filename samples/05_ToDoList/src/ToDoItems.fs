module ToDoItems
open System
open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Fable.Import
open DnnSummit.ToDo.SharedTypes
open Api
open Fable

// Model
type Model =
 {
   Tasks : ToDoItem list option
   NewTask : string option
   ModuleId : int
   CanEdit : bool
   ErrorMessage : string option
 }

type Msgs =
  | ApiMsg of Api.Msgs
  | NewTaskChanged of string
  | AddNewTask
  | Refresh
  | Complete of Id
  | Delete of Id

// Init/ update
let init moduleId  =
  let client  = Api.Client(moduleId) 
  let model = 
    {
      Tasks = None
      NewTask = None
      ModuleId = moduleId
      CanEdit = false
      ErrorMessage = None
    }
  let cmd = Cmd.batch [
              Cmd.map ApiMsg (client.Query ItemsQuery)
              Cmd.map ApiMsg (client.Query CanEditQuery)
             ]
  model, cmd

let NewItem model =
  model.NewTask 
  |> Option.map 
     (fun t -> { Task = t ; Id = System.Guid.NewGuid(); Complete =false })
  

let update msg model =
  
  let client  = Api.Client(model.ModuleId)
  match msg with
  | NewTaskChanged t -> if (String.IsNullOrWhiteSpace t)
                        then {model with NewTask = None}, Cmd.none
                        else {model with NewTask = Some t}, Cmd.none

  | ApiMsg msg -> 
      match msg with

      | Handled command ->
          match command with
          | Command.Add item ->  
              let tasks = model.Tasks |> Option.map (fun tasks -> item :: tasks)
              {model with Tasks = tasks; NewTask= None}, Cmd.none

          | Command.Delete id -> 
              let tasks = model.Tasks 
                          |> Option.map (fun tasks ->tasks |> List.filter (fun t-> t.Id <>id))
              {model with Tasks = tasks}, Cmd.none
          
          | Command.Complete id -> 
              let tasks = 
                match model.Tasks with 
                | Some tasks -> tasks 
                                |> List.map (fun t -> if t.Id = id then {t with Complete = true} else t)
                                |> Some
                | None  -> None
              {model with Tasks = tasks}, Cmd.none   

      | Fetched result ->
         match result with
         | Items tasks  -> {model with Tasks = Some tasks}, Cmd.none    
         | CanEdit ce   -> {model with CanEdit = ce}, Cmd.none   
         
      
      | FetchFailed exn ->{model with ErrorMessage = Some exn.Message}, Cmd.none
  
  | AddNewTask
      -> let cmd =  NewItem model 
                    |> Option.map ( Command.Add >> client.Dispatch )
                    |> Option.defaultValue Cmd.none
         model,  Cmd.map ApiMsg <| cmd 
            
  | Refresh     -> model, Cmd.map ApiMsg (client.Query ItemsQuery)
  | Complete id -> model, Cmd.map ApiMsg (client.Dispatch (Command.Complete id))
  | Delete id   -> model, Cmd.map ApiMsg (client.Dispatch (Command.Delete id))


// View
let inline onValueChange (changed:('s -> unit)) (ev:React.FormEvent)  =  changed !! ev.target?value

let viewTask (item:ToDoItem) canEdit dispatch =
       
  div [ClassName "row"] [
    div [ClassName "col-xs-8"][
      input [
        Type "checkbox"
        Checked ( item.Complete)
        Disabled ( not (canEdit || item.Complete  ))
        OnChange (fun _ ->  dispatch (Complete item.Id))]
      str item.Task ]
    div [ClassName "col-xs-4"][
      if canEdit then 
        yield a [
            ClassName "btn btn-xs btn-default"
            OnClick (fun _ -> dispatch (Delete item.Id))][str "delete" ]
    ]
  ]

let viewNewTask (task: string option) dispatch =
  div[][
    str "New Task:"
    input [ 
        Type "text"
        DefaultValue (task|> Option.defaultValue "") 
        OnChange (onValueChange (NewTaskChanged>>dispatch))
        ]
    a [ 
        ClassName "btn btn-sm btn-primary"
        Disabled task.IsNone 
        OnClick (fun _ -> dispatch AddNewTask) 
        ] 
        [str "Add ToDo"]
  ]


let view (model:Model) dispatch = 

  model.ErrorMessage
  |> Option.map (fun m ->div[][str m])
  |> Option.defaultValue (
    model.Tasks
    |> Option.map (
      fun tasks -> 
        div[][
          yield a [OnClick (fun _ -> dispatch Refresh)][str "Refresh"]
          yield br []
          yield! tasks |> List.map (fun t -> viewTask t model.CanEdit dispatch) 
          if model.CanEdit then yield viewNewTask model.NewTask dispatch
        ] )
    |> Option.defaultValue (div[][str "Loading..."])
  )
  
  
