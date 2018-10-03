using InterestCalculator.Models;
using InterestCalculator.Models.Cards;
using InterestCalculator.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterestCalculator.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        private Calculator _calculator;
        private int _testBalance;
        private int _testBalance2;

        private decimal _visaInterest = .10M;
        private decimal _masterCardInterest = .05M;
        private decimal _discoverInterest = .01M;

        Card _visa;
        Card _visa2;
        Card _masterCard;
        Card _discover;
        Wallet _wallet;
        Wallet _wallet2;
        Person _person;

        [TestInitialize]
        public void Initialize()
        {
            _calculator = new Calculator();
            _wallet = new Wallet();
            _wallet2 = new Wallet();
            _person = new Person();

            _testBalance = 100;
            _testBalance2 = 500;

            _visa = new Visa(_testBalance);
            _visa2 = new Visa(_testBalance2);

            _masterCard = new MasterCard(_testBalance);

            _discover = new Discover(_testBalance);
        }
        
        [TestMethod]
        public void InterestByCard_ReturnsADecimal()
        {
            var interest = _calculator.CalculateInterestForCard(_visa);

            Assert.IsInstanceOfType(interest, typeof(decimal));
        }

        [TestMethod]
        public void InterestByCard_ReturnsCorrectValue_ForVisa()
        {
            var correctInterest = _testBalance * _visaInterest;
            var actualInterest = _calculator.CalculateInterestForCard(_visa);

            Assert.AreEqual(correctInterest, actualInterest);
        }

        [TestMethod]
        public void InterestByCard_ReturnsCorrectValue_ForMastercard()
        {
            var correctInterest = _testBalance * _masterCardInterest;
            var actualInterest = _calculator.CalculateInterestForCard(_masterCard);

            Assert.AreEqual(correctInterest, actualInterest);
        }

        [TestMethod]
        public void InterestByCard_ReturnsCorrectValue_ForDiscover()
        {
            var correctInterest = _testBalance * _discoverInterest;
            var actualInterest = _calculator.CalculateInterestForCard(_discover);

            Assert.AreEqual(correctInterest, actualInterest);
        }

        [TestMethod]
        public void InterestByWallet_ReturnsZero_WhenWalletHasNoCards()
        {
            var interest = _calculator.CalculateInterestForWallet(_wallet);

            Assert.AreEqual(0, interest);
        }

        [TestMethod]
        public void InterestByWallet_ReturnsCorrectValue_WhenWalletHasOneCard()
        {
            _wallet.Cards.Add(_visa);

            var correctInterest = _testBalance * _visaInterest;
            var actualInterest = _calculator.CalculateInterestForWallet(_wallet);

            Assert.AreEqual(correctInterest, actualInterest);
        }

        [TestMethod]
        public void InterestByWallet_ReturnsCorrectValue_WhenWalletHasTwoSameCards()
        {
            _wallet.Cards.Add(_visa);
            _wallet.Cards.Add(_visa2);

            var correctInterest = _testBalance * _visaInterest + _testBalance2 * _visaInterest;
            var actualInterest = _calculator.CalculateInterestForWallet(_wallet);

            Assert.AreEqual(correctInterest, actualInterest);
        }

        [TestMethod]
        public void InterestByWallet_ReturnsCorrectValue_WhenWalletHasTwoDifferentCards()
        {
            _wallet.Cards.Add(_visa);
            _wallet.Cards.Add(_masterCard);

            var correctInterest = _testBalance * _visaInterest + _testBalance * _masterCardInterest;
            var actualInterest = _calculator.CalculateInterestForWallet(_wallet);

            Assert.AreEqual(correctInterest, actualInterest);
        }

        [TestMethod]
        public void InterestByPerson_ReturnsZero_WhenPersonHasNoWallets()
        {
            var interest = _calculator.CalculateInterestForPerson(_person);

            Assert.AreEqual(0, interest);
        }

        [TestMethod]
        public void InterestByPerson_ReturnsCorrectValue_WhenPersonHasOneWallet()
        {
            _wallet.Cards.Add(_visa);
            _wallet.Cards.Add(_masterCard);
            _wallet.Cards.Add(_discover);
            _person.Wallets.Add(_wallet);

            var correctInterest = _testBalance * _visaInterest 
                + _testBalance * _masterCardInterest + _testBalance * _discoverInterest;

            var actualInterest = _calculator.CalculateInterestForPerson(_person);

            Assert.AreEqual(correctInterest, actualInterest);
        }

        [TestMethod]
        public void InterestByPerson_ReturnsCorrectValue_WhenPersonHasTwoWallets()
        {
            _wallet.Cards.Add(_visa);
            _wallet.Cards.Add(_masterCard);

            _wallet2.Cards.Add(_visa2);
            _wallet2.Cards.Add(_discover);

            _person.Wallets.Add(_wallet);
            _person.Wallets.Add(_wallet2);

            var correctInterest = _testBalance * _visaInterest + _testBalance * _masterCardInterest
                + _testBalance2 * _visaInterest + _testBalance * _discoverInterest;

            var actualInterests = _calculator.CalculateInterestForPerson(_person);

            Assert.AreEqual(correctInterest, actualInterests);
        }

        [TestMethod]
        public void TestCase1()
        {
            var visa = new Visa(100);
            var mc = new MasterCard(100);
            var discover = new Discover(100);

            _wallet.Cards.Add(visa);
            _wallet.Cards.Add(mc);
            _wallet.Cards.Add(discover);

            _person.Wallets.Add(_wallet);

            var correctVisaInterest = _visaInterest * 100;
            var correctMcInterest = _masterCardInterest * 100;  
            var correctDiscoverInterest = _discoverInterest * 100;
            var correctTotalInterest = correctVisaInterest + correctMcInterest + correctDiscoverInterest;

            var actualVisaInterest = _calculator.CalculateInterestForCard(visa);
            var actualMcInterest = _calculator.CalculateInterestForCard(mc);
            var actualDiscoverInterest = _calculator.CalculateInterestForCard(discover);
            var actualTotalInterest = _calculator.CalculateInterestForPerson(_person);

            Assert.AreEqual(correctVisaInterest, actualVisaInterest);
            Assert.AreEqual(correctMcInterest, actualMcInterest);
            Assert.AreEqual(correctDiscoverInterest, actualDiscoverInterest);
            Assert.AreEqual(correctTotalInterest, actualTotalInterest);
        }

        [TestMethod]
        public void TestCase2()
        {
            _wallet.Cards.Add(new Visa(100));
            _wallet.Cards.Add(new Discover(100));
            _wallet2.Cards.Add(new MasterCard(100));

            _person.Wallets.Add(_wallet);
            _person.Wallets.Add(_wallet2);

            var correctWallet1Interest = 100 * _visaInterest + 100 * _discoverInterest;
            var correctWallet2Interest = 100 * _masterCardInterest;
            var correctTotalInterest = correctWallet1Interest + correctWallet2Interest;

            var actualWallet1Interest = _calculator.CalculateInterestForWallet(_wallet);
            var actualWallet2Interest = _calculator.CalculateInterestForWallet(_wallet2);
            var actualTotalInterest = _calculator.CalculateInterestForPerson(_person);

            Assert.AreEqual(correctWallet1Interest, actualWallet1Interest);
            Assert.AreEqual(correctWallet2Interest, actualWallet2Interest);
            Assert.AreEqual(correctTotalInterest, actualTotalInterest);
        }

        [TestMethod]
        public void TestCase3()
        {
            _wallet.Cards.Add(new Visa(100));
            _wallet.Cards.Add(new MasterCard(100));

            _wallet2.Cards.Add(new Visa(100));
            _wallet2.Cards.Add(new MasterCard(100));

            _person.Wallets.Add(_wallet);

            var person2 = new Person();
            person2.Wallets.Add(_wallet2);

            var correctInterest = 100 * _visaInterest + 100 * _masterCardInterest;

            var actualWallet1Interest = _calculator.CalculateInterestForWallet(_wallet);
            var actualWallet2Interest = _calculator.CalculateInterestForWallet(_wallet2);
            var actualPerson1Interest = _calculator.CalculateInterestForPerson(_person);
            var actualPerson2Interest = _calculator.CalculateInterestForPerson(person2);

            Assert.AreEqual(correctInterest, actualWallet1Interest);
            Assert.AreEqual(correctInterest, actualWallet2Interest);
            Assert.AreEqual(correctInterest, actualPerson1Interest);
            Assert.AreEqual(correctInterest, actualPerson2Interest);
        }
    }
}
