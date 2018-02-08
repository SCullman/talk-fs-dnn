module Api

open Elmish
open Fable.PowerPack
open Fable.PowerPack.Fetch

type Msgs =
  | FetchFailed of exn
  | GreetingFetched of string
  
type Client (moduleId) =
  let sf = Dnn.ServiceFramework moduleId
  let headers = Dnn.moduleHeaders sf
  let url  = sf.getServiceRoot "dnnsummit" + "simple"

  let fetchGreeting (name) =
    promise {
      let url =  sprintf "%s/greet?name=%s" url name    
      let props = [ RequestProperties.Method HttpMethod.GET
                    Fetch.requestHeaders(headers) ]
      return! Fetch.fetchAs<string> url props
    }
    
with
member __.GetGreeting name = Cmd.ofPromise fetchGreeting name GreetingFetched FetchFailed
