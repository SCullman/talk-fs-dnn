module DnnSummit.ToDo.SharedTypes
open System.Collections.Generic

type Id = System.Guid

type ToDoItem = 
 {
    Id:Id
    Task : string
    Complete : bool
 }

 type Command =
   | Add of ToDoItem
   | Delete of Id
   | Complete of Id

 type Query =
   | ItemsQuery
   | CanEditQuery

 type QueryResult =
   | Items of ToDoItem list
   | CanEdit of bool
   
