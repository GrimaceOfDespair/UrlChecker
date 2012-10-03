UrlChecker
==========

UrlChecker reads urls, places web requestes and outputs the status code after following any 301 or 302 redirects. The output format are 3 tab-delimited columns containing the original url, the status code and the eventual url.

The output of UrlChecker can be fed directly back into UrlChecker to recheck.


Example usage
=============

    UrlChecker
 
        type in url (http://foo.bar) to check response code
    
 
    UrlChecker <urls.txt >ok.txt 2>error.txt
   
        Generate 2 files divided into working and broken urls.
        urls.txt is a list of urls to check.
        ok.txt will contain all 200 responses
        error.txt will contain all other responses
     
     
    UrlChecker <urls.txt >nul

        Generate a list of broken urls.
        urls.txt is a list of urls to check.

   
    UrlChecker <urls.txt | UrlChecker

        Pretty pointless proof of concept to show that
        the output is readable by UrlChecker again
