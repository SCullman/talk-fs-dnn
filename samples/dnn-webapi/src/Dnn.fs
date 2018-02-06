module Dnn

open Fable.Core
open Fable.Import.Browser

let getModuleId elementId =  
  (document.getElementById elementId).dataset.["moduleid"]
  |> System.Int32.Parse

type IServicesFramework = 
  abstract getServiceRoot: string     -> string 
  abstract setModuleHeaders: obj      -> unit
  abstract getTabId : unit            -> int option
  abstract getModuleId : unit         -> int option 
  abstract getAntiForgeryValue : unit -> string option
  

[<Emit("window['$'].ServicesFramework($0)")>]
let ServiceFramework (moduleid:int) : IServicesFramework  = jsNative 

open Fable.PowerPack.Fetch.Fetch_types

let moduleHeaders (sf:IServicesFramework) =
  [ 
    HttpRequestHeaders.Custom ("ModuleId", sf.getModuleId())
    HttpRequestHeaders.Custom ("TabId", sf.getTabId())
    HttpRequestHeaders.Custom ("RequestVerificationToken", sf.getAntiForgeryValue())
  ]
     