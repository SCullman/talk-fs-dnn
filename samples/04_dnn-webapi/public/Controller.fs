namespace DnnSummit.WebApi

open System.Web.Http
open System.Web.Http.Controllers
open System.Net
open System.Net.Http
open System.Net.Http.Formatting
open DotNetNuke.Web.Api
open Newtonsoft.Json


type FableConfigAttribute () =
  inherit System.Attribute()

  interface IControllerConfiguration with
    member __.Initialize (controllerSettings, _) =
        let fableFormatter = JsonMediaTypeFormatter ( SerializerSettings = JsonSerializerSettings (Converters = [|Fable.JsonConverter()|]))
        controllerSettings.Formatters.Clear ()
        controllerSettings.Formatters.Add fableFormatter

[<FableConfig>]
type FableController () =
  inherit DnnApiController ()

type RouteMapper ()=
  interface IServiceRouteMapper with
    member __.RegisterRoutes (rtm:IMapRoute) = 
      let namespaces = [|"DnnSummit.WebApi"|]
      rtm.MapHttpRoute  ("dnnsummit", "default", "{controller}/{action}", namespaces) |>ignore           

type  WelcomeController () =
  inherit  DnnApiController ()

  [<HttpGet>]
  [<AllowAnonymous>]
  member __.HelloWorld () = 
    __.Request.CreateResponse(HttpStatusCode.OK, "Hello World")


type SimpleController  () =
  inherit FableController () 
  
  [<HttpGet>]
  [<AllowAnonymous>]
  member __.Greet (name:string) =
      let result = sprintf "Hello %s" name
      __.Request.CreateResponse (HttpStatusCode.OK, result) 
