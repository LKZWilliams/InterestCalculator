using InterestCalculator.Models;

namespace InterestCalculator
{
    public class Calculator
    {
        public decimal CalculateInterestForPerson(Person person)
        {
            decimal totalInterest = 0;

            foreach(var wallet in person.Wallets)
            {
                totalInterest += CalculateInterestForWallet(wallet);
            }

            return totalInterest;
        }

        public decimal CalculateInterestForWallet(Wallet wallet)
        {
            decimal totalInterest = 0;

            foreach (var card in wallet.Cards)
            {
                totalInterest += CalculateInterestForCard(card);
            }

            return totalInterest;
        }

        public decimal CalculateInterestForCard(Card card)
        {
            return card.Balance * card.InterestRate;
        }
    }
}
