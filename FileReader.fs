module AdventDay16.FileReader

open System.IO
open Domain
     
let mapToRange (bounds:string) =
    let boundsArray = bounds.Split "-"
    { lower = (int boundsArray.[0]); upper = (int boundsArray.[1]) }
    
let readRangeString (rangeString: string) =
    let nameAndRanges = rangeString.Split ": "
    let ranges =
        nameAndRanges.[1].Split " or "
        // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/Pipeline_operator
        // nameAndRanges[1].split(" or ").map(mapToRange)
        |> Array.map mapToRange
    { name = nameAndRanges.[0]; ranges = ranges  }

let readRanges() =
    let fileContent = File.ReadAllLines "ranges.txt"
    fileContent
    |> Array.map readRangeString

let readTicket (ticketString: string): Ticket =
    ticketString.Split ","
    |> Array.map int
   
let readMyTicket() =
    File.ReadAllText "myTicket.txt"
    |> readTicket
    
let readNearbyTickets() =
    File.ReadAllLines "nearbyTickets.txt"
    |> Array.map readTicket