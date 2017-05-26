Imports StarCasino.Core.Domain

Namespace StarCasino.Core

    Public Class User

        Dim Id As Guid
        Dim Nickname As String
        Dim Wallet As Account
        Dim Rank As Loyalty

        Sub New(Byval id As Guid, Byval nickname As String, ByVal wallet As Account, ByVal rank As Loyalty)
            Id = id
            Nickname = nickname
            Wallet = wallet
            Rank = rank
        End Sub

        Public Shared Function NewPlayer(ByVal nickname As String)
            Dim balance = New Tuple(Of Decimal, Decimal)(0, 0)
            Dim wallet = New Account(Guid.NewGuid(), balance)
            Return New User(Guid.NewGuid(), nickname, wallet, Loyalty.Standard)
        End Function

        Public Sub SettleBet(ByVal betState As Integer, ByVal amount As Decimal)
            Dim action = convertBetState(betState, amount)
            Dim balance = settleBalance(Wallet.Balance.Item1, Wallet.Balance.Item2, action)
            Wallet = New Account(Wallet.Id, balance)
        End Sub

        Public Sub SettleBetTwo(ByVal betState As Integer, ByVal amount As Decimal)
            Dim action = convertBetState(betState, amount)
            Dim silver = Microsoft.FSharp.Core.FSharpFunc(Of BalanceAction, Tuple(Of Decimal, Decimal)) _
                            .FromConverter( _
                                New Converter(Of BalanceAction, Tuple(Of Decimal, Decimal))( _
                                    AddressOf SettleBalanceLoyaltySilver))

            Dim balance = settleBalanceTwo(Wallet.Balance.Item1, Wallet.Balance.Item2, action, silver)
            Wallet = New Account(Wallet.Id, balance)
        End Sub


        Public Function SettleBalanceLoyaltySilver()
            Dim silver As Func(Of BalanceAction, Tuple(Of Decimal, Decimal)) =
                Function(action)
                    If action.IsAdd Then
                        Dim amount = CType(action, BalanceAction.Add).Item
                        Dim split = amount / 2
                        Return New Tuple(Of Decimal, Decimal)(split, split)
                    ElseIf action.IsSubstract 
                        Dim amount = CType(action, BalanceAction.Substract).Item
                        Dim split = - (amount / 2)
                        Return New Tuple(Of Decimal, Decimal)(split, split)
                    Else
                        Return New Tuple(Of Decimal, Decimal)(0, 0)
                    End If
                End Function

            Return silver
        End Function

    End Class
End Namespace
