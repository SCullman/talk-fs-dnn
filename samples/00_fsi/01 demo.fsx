type Package =
  | Skiing
  | Snowboarding
  | Snowmobiling 

type Person = {
  Name: string
  Package: Package
}

let persons = [
    {Name="Stefan"; Package= Skiing}
    {Name="Erik"; Package= Snowmobiling}
    {Name="Shaun"; Package= Skiing} ]

persons
|> List.filter (fun p -> p.Name.Contains "r" )
|> List.iter (
  fun p ->
    match p.Package with
    | Skiing -> printfn "%s is skiing" p.Name
    | _ -> printfn "%s is not skiing" p.Name
)
