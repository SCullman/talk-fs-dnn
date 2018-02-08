// ## VALUES

// bind 5 to x
let x:int = 5

// type inference
let inferredX = 5

// functions are just values, don't need a class
let helloWorld name = sprintf "Hello, %s" name 

// type inference again
let text = helloWorld "DNN-Summit"

// ## TYPES

// Tuples are first class citizens in F#
let person = System.Tuple.Create ("Stefan", 50)
let personShortHand = "Stefan", 50 // string * int
let name, age = personShortHand // decompose the tuple 

// Declaring a record
type Person = { Name : string; Age : int }

// Create an instance
let me = { Name = "Stefan"; Age = 50 }
printfn "%s is %d years old" me.Name me.Age


// ## Immutable by default

let a = 10
//a <- 20 // not allowed

let mutable y = 10 // need an extra keyword!
y <- 20 // ok 

//me.Age <- 20 // not allowed

//creates a new record based on an existing one
let youngerme = {me with Age = 20} 