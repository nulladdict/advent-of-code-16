module AdventDay16.Main

open System.Numerics
open AdventDay16.FileReader
open AdventDay16.Domain

let valueInRange value range =
    range.lower <= value && range.upper >= value

let rangeTypeMatchesValue value (rangeType:RangeType)  =
    rangeType.ranges
    |> Array.exists (valueInRange value)

let rangeTypeMatchesAll values (rangeType:RangeType)  =
    values
    |> Array.forall (fun v -> rangeTypeMatchesValue v rangeType)

let findRangeTypeMatchesAll (ranges: RangeType array) (values: int array)  =
    ranges
    |> Array.filter (rangeTypeMatchesAll values)

let ticketScanningErrorRate (ranges: RangeType array) (ticket: Ticket)  =
    ticket
    |> Array.filter (fun v -> ranges |> Array.exists (rangeTypeMatchesValue v) |> not)
    |> Array.sum
    
let isValid (ranges: RangeType array) (ticket: Ticket)  =
    ticket
    |> Array.forall (fun v -> ranges |> Array.exists (rangeTypeMatchesValue v))
    
let ticketsColumns (tickets: Ticket array) =
    let tickets2D = array2D tickets
    seq { 0 ..tickets.[0].Length - 1  }
    |> Seq.map (fun c -> tickets2D.[*, c])
    |> Seq.toArray

let rec filterUniques (possibleColumns: RangeType[][]) =
    let uniqueColumns = possibleColumns |> Array.filter(fun v -> v.Length = 1) |> Array.collect id
    let result =
        possibleColumns
        |> Array.map(fun vars ->
                    vars
                    |> Array.filter (fun v -> vars.Length = 1 || not (Array.contains v uniqueColumns)))
    if (result |> Array.exists (fun a -> a.Length > 1)) then filterUniques result else result

let myTicket = readMyTicket()

[<EntryPoint>]
let main argv =
    let ranges = readRanges()
    let nearbyTickets = readNearbyTickets()
    let myTicket = readMyTicket()
    
    let resultColumns =
        nearbyTickets
        |> Array.filter (isValid ranges)
        |> ticketsColumns
        |> Array.map (findRangeTypeMatchesAll ranges)
        |> filterUniques
        
    resultColumns
        |> Array.mapi (fun i c -> i, c.[0].name)
        |> Array.filter (fun (_, c) -> c.StartsWith "departure")
        |> Array.map (fun (i, _) -> bigint myTicket.[i])
        |> Array.reduce (fun p n -> bigint.Multiply(p, n))
        |> fun res -> printfn "И наше заветное число - %s" (res.ToString())
    0
