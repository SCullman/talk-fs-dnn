module Api

open Elmish
open Fable.PowerPack
open Fable.PowerPack.Fetch
open DnnSummit.ToDo.SharedTypes
open Fable.Core.JsInterop

type Msgs =
  | Handled of Command
  | Fetched of QueryResult
  | FetchFailed of exn

  
type Client (moduleId) =
  let sf = Dnn.ServiceFramework moduleId
  let headers = Dnn.moduleHeaders sf
  let url  = sf.getServiceRoot "SummitToDo" + "item"


  let dispatch (cmd:Command) =
    promise {
      let url =  sprintf "%s/handle" url     
      let props = [ RequestProperties.Method HttpMethod.POST
                    Fetch.requestHeaders (ContentType "application/json" :: headers)
                    Credentials RequestCredentials.Sameorigin
                    Body !^(toJson cmd) ]               
      do Fetch.fetch url props|> ignore
      return cmd
    }



  let query (query:Query) = 
    promise {
      let url =  sprintf "%s/execute" url      
      let props = [ RequestProperties.Method HttpMethod.POST
                    Fetch.requestHeaders (ContentType "application/json" :: headers)
                    Credentials RequestCredentials.Sameorigin
                    Body !^(toJson query) ]   
      return! Fetch.fetchAs<QueryResult> url props
    }

with
member __.Dispatch cmd = Cmd.ofPromise dispatch cmd Handled FetchFailed
member __.Query cmd    = Cmd.ofPromise query cmd Fetched FetchFailed
