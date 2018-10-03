namespace InterestCalculator.Models.Cards
{
    public class Discover : Card
    {
        public Discover(decimal balance)
        {
            InterestRate = .01M;
            Balance = balance;
        }
    }
}
