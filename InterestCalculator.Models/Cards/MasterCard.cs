namespace InterestCalculator.Models.Cards
{
    public class MasterCard : Card
    {
        public MasterCard(decimal balance)
        {
            InterestRate = .05M;
            Balance = balance;
        }
    }
}
