module Elmish

open Elmish
open Elmish.React
open Elmish.HMR

open ToDoItems

let elementId = "elmish-todo"

let init = 
  let moduleId = Dnn.getModuleId elementId
  fun () -> init moduleId
 
Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact elementId
|> Program.run