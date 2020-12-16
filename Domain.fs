module AdventDay16.Domain

type Range = { lower: int; upper: int; }

type RangeType = { name: string; ranges: Range array  }

type Ticket = int array