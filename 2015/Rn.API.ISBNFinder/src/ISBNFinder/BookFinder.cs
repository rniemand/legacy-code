using System;
using Rn.Common.Interfaces;

namespace Rn.API.ISBNFinder
{
    public class BookFinder
    {
        private readonly IWebClient _webClient;

        public BookFinder(IWebClient webClient)
        {
            _webClient = webClient;
        }



        public void Find(string isbn)
        {
            Console.WriteLine(_webClient.DownloadAsString(""));


            /*
             http://isbndb.com/api/v1/docs/keys
                -> 2NR3K7M2

                http://isbndb.com/api/books.xml?access_key=2NR3K7M2&index1=isbn&value1=9781406328073
             */

        }
    }
}
