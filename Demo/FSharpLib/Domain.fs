namespace StarCasino.Core

module Domain =

    open System

    type Balance = decimal * decimal

    type Account =
        { Id : Guid
          Balance : Balance }

    // converts to class
    type Loyalty =
        | Standard
        | Brons
        | Silver
        | Gold

    // converts to a class
    type BalanceAction =
        | Add of decimal
        | Substract of decimal
        | Nothing

    // converts to FsharpChoice<Unit, Unit, Unit, Unit>
    let (|Cancelled|Lost|Won|Unknown|) betState =
        match betState with
        | 1 -> Cancelled
        | 2 -> Lost
        | 3 -> Won
        | _ -> Unknown


    let convertBetState betState amount =
        match betState with
        | Cancelled -> Add(amount)
        | Lost -> Substract(amount)
        | Won -> Add(amount)
        | Unknown -> Nothing


    let settleBalance balance action =
        match action with
        | Add x ->
            let bonus = (balance |> snd) + x
            (balance |> fst, bonus)
        | Substract x ->
            let cash = (balance |> fst) - x
            (cash, balance |> snd)
        | Nothing -> balance

    
    //gold settle bet    
    
    type LoyaltySettleBalance = BalanceAction -> (decimal * decimal)
    
    let goldSettleBalance action =
        match action with
        | Add x -> (x, 0.0m)
        | Substract x -> (0.0m, -x)
        | Nothing -> (0.0m, 0.0m)
    
    let settleBalanceTwo balance action (settle : LoyaltySettleBalance) =
        let toAdd = settle action
        let cash = (balance |> fst) + (toAdd |> fst)
        let bonus = (balance |> snd) + (toAdd |> snd)
        (cash, bonus)

    // property
    let add10 = (10.0, 0.0)

    // method
    let add20 () = (20.0, 0.0)
