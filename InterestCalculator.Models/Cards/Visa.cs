namespace InterestCalculator.Models.Cards
{
    public class Visa : Card
    {
        public Visa(decimal balance)
        {
            InterestRate = .10M;
            Balance = balance;
        }

    }
}
