using System;
using Microsoft.FSharp.Core;
using static StarCasino.Core.Domain;

namespace StarCasino.Core {

    public class User {

        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public Account Wallet { get; set; }
        public Loyalty Rank { get; set; }

       public User(Guid id, string nickname, Account wallet, Loyalty rank) {
            Id = id;
            Nickname = nickname;
            Wallet = wallet;
            Rank = rank;
        }

        public void SettleBet(int betState, decimal amount) {
            // convert betstate
            var action = convertBetState(betState, amount);

            // settle balance
            var balance = settleBalance(Wallet.Balance.Item1, Wallet.Balance.Item2, action);

            // create account
            Wallet = new Account(Wallet.Id, balance);           
        }

        private Func<BalanceAction, Tuple<decimal, decimal>> SettleBalanceLoyaltySilver() {
            Func<BalanceAction, Tuple<decimal, decimal>> settleSilverBalance = action => {
                if (action.IsAdd) {
                    var amount = ((BalanceAction.Add)action).Item;
                    var split = amount / 2;
                    return new Tuple<decimal, decimal>(split, split);
                }
                else if (action.IsSubstract) {
                    var amount = ((BalanceAction.Substract)action).Item;
                    var split = -(amount / 2);
                    return new Tuple<decimal, decimal>(split, split);
                }
                else {
                    return new Tuple<decimal, decimal>(0.0m, 0.0m);
                }
            };
            return settleSilverBalance;
        }

        public void SettleBetTwo(int betState, decimal amount) {
            var action = convertBetState(betState, amount);
            var silver =
                Microsoft.FSharp.Core.FSharpFunc<BalanceAction, Tuple<decimal, decimal>>
                    .FromConverter(new Converter<BalanceAction, Tuple<decimal, decimal>>(SettleBalanceLoyaltySilver()));

            var balance = settleBalanceTwo(Wallet.Balance.Item1, Wallet.Balance.Item2, action, silver);
            Wallet = new Account(Wallet.Id, balance);
        }


        public static User New(string nickname) {
            var balance = new Tuple<decimal, decimal>(100, 100);
            var account = new Account(Guid.NewGuid(), balance);
            return new User(Guid.NewGuid(), nickname, account, Loyalty.Standard);
        }
    }
}
