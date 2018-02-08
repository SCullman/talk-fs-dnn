module Elmish

open Elmish
open Elmish.React
open Elmish.HMR

open Hello

let elementId = "elmish-app"

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