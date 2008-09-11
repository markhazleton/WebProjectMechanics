# Standardista Table Sorting v1.1 Readme #

Standardista Table Sorting is a JavaScript module that lets you sort an HTML 
data table by any column.  The module has the following advantages:

1.  **It works**.  

    The codebase has been tested across a large number of web browsers, and in 
    the few that won't allow the sorting to occur, the module degrades 
    gracefully.  In these cases the original table is not altered in any way, 
    shape or form.
   
2.  **It's unobtrusive**.  

    You add a reference to the Standardista Table Sorting JavaScript files in 
    the head of your webpage, add a class of "sortable" to the tables in your 
    page that should be sorted and you're done.

3.  **It's fast**. 

    Sorting a column in an eight column, one hundred row, table takes on 
    average between 100 and 200 milliSeconds in my testing. That's just a tenth 
    of a second for a bigger table than you're often likely to put on a 
    webpage. 
   
4.  **It knows about different data types**.  
   
    The current version of Standardista Table Sorting will sort IP Addresses, 
    Currency, Number, Plain Text and Dates, and the best bit is it works out 
    what type of sorting to do without you having to tell it.  If you know a
    small amount of JavaScript it's also trivial to add new data types to sort
    by.
    
Standardista Table Sorting was first released in February 2006, and is based on
Stuart Langridge's "sorttable" code.  Specifically, the determineSortFunction, 
sortCaseInsensitive, sortDate, sortNumeric, and sortCurrency functions are 
heavily based on his code.  This module would not have been possible without 
Stuart's earlier outstanding work.

For a detailed list of changes to Standardista Table Sorting over time, please
refer to the [changelog][1]
    
## How to use Standardista Table Sorting ##

1.  First, download the [Standardista Table Sorting module][2].

2.  Include the .js files in the HEAD of your webpage:

        <script type='text/javascript' src='common.js'></script>
        <script type='text/javascript' src='css.js'></script>
        <script type='text/javascript' src='standardista-table-sorting.js'></script>

3.  Make sure that the table you want to be sortable has a THEAD and a TBODY.

4.  Give the table that wants to be able to be sorted the class "sortable".

5.  Load the page, and you'll be able to sort the table by clicking on its 
    headers!

## Extended Pointers for using Standardista Table Sorting ##

-   Every table that you want users to be able to sort **must** contain a THEAD 
    and a TBODY section.  If you're using tables for displaying data, this is 
    just good practice anyway.  If you aren't, then it's not a huge hardship to 
    wrap your heading row in a THEAD and your data rows in a TBODY, is it?
    
-   If you have more than one row in your THEAD then the last row will be the 
    one that receives the links which the user can use to sort the table.
    
-   If you want to leave a row (or more) of data sitting statically at the 
    bottom of the table, then put it in a TFOOT section.  Remember, the TFOOT 
    should come *between* the THEAD and the TBODY in your HTML source.

[1]: http://www.workingwith.me.uk/standardista/changelog
[2]: http://www.workingwith.me.uk/downloads/standardista-table-sorting.zip