module Counters

open Fable.Helpers.React
open Fable.Helpers.React.Props

// model

type Model = Counter.Model list

type Msg =
  | Modify of int * Counter.Msg
  | Insert 
  | Remove 

// init & update state

let init () =
  [ 0 ]

let update msg model =
  match msg with
  | Insert -> 0 :: model
  
  | Remove -> 
      match model with
      | [] -> []
      | x::rest -> rest

  | Modify (index, counterMsg) -> 
      model 
      |> List.mapi (
         fun i c -> if i = index
                     then Counter.update counterMsg c
                     else c)
// View
let view model dispatch =
   
   let counterDispatch i msg = Modify (i,msg) |> dispatch

   let counters =  
      model 
      |> List.mapi (fun i counter -> Counter.view counter (counterDispatch i))
   
   div [][
     yield! counters
     yield button [OnClick (fun ev -> dispatch (Insert))][str "Insert counter"]
     yield button [OnClick (fun ev -> dispatch (Remove))][str "Remove counter"]
   ]

  



