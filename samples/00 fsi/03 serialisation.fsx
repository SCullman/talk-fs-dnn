#r @"c:\Users\root\.nuget\packages\fable.jsonconverter\1.0.7\lib\net45\Fable.JsonConverter.dll"
#r @"C:\Users\root\.nuget\packages\newtonsoft.json\10.0.3\lib\net45\Newtonsoft.Json.dll"

open Newtonsoft.Json

type Gender = Male | Female

type Person = {
  Name : string
  Gender: Gender 
  Age: int option
}

let p = {
  Name = "Stefan"
  Gender = Male
  Age = Some 50
}

let serializeWithJsonNet o = JsonConvert.SerializeObject o
let jsonConverter = Fable.JsonConverter() :> JsonConverter
let serializeWithFable o = JsonConvert.SerializeObject (o,  [|jsonConverter|])
//
serializeWithJsonNet p
serializeWithFable p
serializeWithFable {p with Age = None}
