namespace DnnSummit.ToDo

open DotNetNuke.Web.Api
open System.Web.Http
open System.Net
open System.Net.Http
open DotNetNuke.Entities.Portals
open Newtonsoft.Json
open System.Net.Http.Formatting
open System.Web.Http.Controllers
open SharedTypes

type FableConfigAttribute () =
  inherit System.Attribute()

  interface IControllerConfiguration with
    member __.Initialize ((controllerSettings:HttpControllerSettings), _ ) =
        let fableFormatter = 
          JsonMediaTypeFormatter ( SerializerSettings = 
            JsonSerializerSettings (Converters = [|Fable.JsonConverter()|]))
        controllerSettings.Formatters.Clear ()
        controllerSettings.Formatters.Add fableFormatter

[<FableConfig>]
type FableController () =
  inherit DnnApiController ()

type RouteMapper ()=
  interface IServiceRouteMapper with
    member __.RegisterRoutes (rtm:IMapRoute) = 
      let namespaces = [|"DnnSummit.ToDo"|]
      rtm.MapHttpRoute  ("SummitToDo", "default", "{controller}/{action}", namespaces) |>ignore           

type ItemController  () =
  inherit FableController ()

  [<HttpPost>]
  [<ValidateAntiForgeryToken>]
  [<DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)>]
  member __.execute query = 
    let result = 
        match query with 
        | ItemsQuery
           -> Logic.getList __.ActiveModule.ModuleID |> Items
        | CanEditQuery 
           ->(__.UserInfo <> null && __.UserInfo.IsInRole "Registered Users") |> CanEdit
    __.Request.CreateResponse (HttpStatusCode.OK, result) 

  [<HttpPost>]
  [<ValidateAntiForgeryToken>]
  [<DnnAuthorize (StaticRoles = "Registered Users")>]
  member __.handle cmd = 
    match cmd with 
    | Add item    -> Logic.add item __.ActiveModule.ModuleID __.UserInfo.UserID
    | Delete id   -> Logic.delete id
    | Complete id -> Logic.complete id  __.UserInfo.UserID
    __.Request.CreateResponse HttpStatusCode.OK 