module Dnn

open Fable.Core
open Fable.PowerPack.Fetch.Fetch_types

type IServicesFramework = 
  abstract getServiceRoot: string     -> string 
  abstract setModuleHeaders: obj      -> unit
  abstract getTabId : unit            -> int option
  abstract getModuleId : unit         -> int option 
  abstract getAntiForgeryValue : unit -> string option

[<Emit("window['$'].ServicesFramework($0)")>]
let ServiceFramework moduleid : IServicesFramework  = jsNative 

let moduleHeaders (sf:IServicesFramework)  = 
  [ 
    HttpRequestHeaders.Custom ("ModuleId", sf.getModuleId())
    HttpRequestHeaders.Custom ("TabId", sf.getTabId())
    HttpRequestHeaders.Custom ("RequestVerificationToken", sf.getAntiForgeryValue())
  ]