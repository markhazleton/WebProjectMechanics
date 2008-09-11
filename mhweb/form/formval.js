// ----------------------------------------------------------------------
// Javascript form validation routines.
// Author: Stephen Poley
//
// Simple routines to quickly pick up obvious typos.
// All validation routines return true if executed by an older browser:
// in this case validation must be left to the server.
//
// Update Jun 2005: discovered that reason IE wasn't setting focus was
// due to an IE timing bug. Added 0.1 sec delay to fix.
//
// Update Oct 2005: minor tidy-up: unused parameter removed
//
// Update Jun 2006: minor improvements to variable names and layout
// ----------------------------------------------------------------------

var nbsp = 160;		// non-breaking space char
var node_text = 3;	// DOM text node-type
var emptyString = /^\s*$/ ;
var global_valfield;	// retain valfield for timer thread

// --------------------------------------------
//                  trim
// Trim leading/trailing whitespace off string
// --------------------------------------------

function trim(str)
{
  return str.replace(/^\s+|\s+$/g, '');
}


// --------------------------------------------
//                  setfocus
// Delayed focus setting to get around IE bug
// --------------------------------------------

function setFocusDelayed()
{
  global_valfield.focus();
}

function setfocus(valfield)
{
  // save valfield in global variable so value retained when routine exits
  global_valfield = valfield;
  setTimeout( 'setFocusDelayed()', 100 );
}


// --------------------------------------------
//                  msg
// Display warn/error message in HTML element.
// commonCheck routine must have previously been called
// --------------------------------------------

function msg(fld,     // id of element to display message in
             msgtype, // class to give element ("warn" or "error")
             message) // string to display
{
  // setting an empty string can give problems if later set to a 
  // non-empty string, so ensure a space present. (For Mozilla and Opera one could 
  // simply use a space, but IE demands something more, like a non-breaking space.)
  var dispmessage;
  if (emptyString.test(message)) 
    dispmessage = String.fromCharCode(nbsp);    
  else  
    dispmessage = message;

  var elem = document.getElementById(fld);
  elem.firstChild.nodeValue = dispmessage;  
  
  elem.className = msgtype;   // set the CSS class to adjust appearance of message
}

// --------------------------------------------
//            commonCheck
// Common code for all validation routines to:
// (a) check for older / less-equipped browsers
// (b) check if empty fields are required
// Returns true (validation passed), 
//         false (validation failed) or 
//         proceed (don't know yet)
// --------------------------------------------

var proceed = 2;  

function commonCheck    (valfield,   // element to be validated
                         infofield,  // id of element to receive info/error msg
                         required)   // true if required
{
  if (!document.getElementById) 
    return true;  // not available on this browser - leave validation to the server
  var elem = document.getElementById(infofield);
  if (!elem.firstChild) return true;  // not available on this browser 
  if (elem.firstChild.nodeType != node_text) return true;  // infofield is wrong type of node  

  if (emptyString.test(valfield.value)) {
    if (required) {
      msg (infofield, "error", "ERROR: required");  
      setfocus(valfield);
      return false;
    }
    else {
      msg (infofield, "warn", "");   // OK
      return true;  
    }
  }
  return proceed;
}

// --------------------------------------------
//            validatePresent
// Validate if something has been entered
// Returns true if so 
// --------------------------------------------

function validatePresent(valfield,   // element to be validated
                         infofield ) // id of element to receive info/error msg
{
  var stat = commonCheck (valfield, infofield, true);
  if (stat != proceed) return stat;

  msg (infofield, "warn", "");  
  return true;
}

// --------------------------------------------
//               validateEmail
// Validate if e-mail address
// Returns true if so (and also if could not be executed because of old browser)
// --------------------------------------------

function validateEmail  (valfield,   // element to be validated
                         infofield,  // id of element to receive info/error msg
                         required)   // true if required
{
  var stat = commonCheck (valfield, infofield, required);
  if (stat != proceed) return stat;

  var tfld = trim(valfield.value);  // value of field with whitespace trimmed off
  var email = /^[^@]+@[^@.]+\.[^@]*\w\w$/  ;
  if (!email.test(tfld)) {
    msg (infofield, "error", "ERROR: not a valid e-mail address");
    setfocus(valfield);
    return false;
  }

  var email2 = /^[A-Za-z][\w.-]+@\w[\w.-]+\.[\w.-]*[A-Za-z][A-Za-z]$/  ;
  if (!email2.test(tfld)) 
    msg (infofield, "warn", "Unusual e-mail address - check if correct");
  else
    msg (infofield, "warn", "");
  return true;
}


// --------------------------------------------
//            validateTelnr
// Validate telephone number
// Returns true if so (and also if could not be executed because of old browser)
// Permits spaces, hyphens, brackets and leading +
// --------------------------------------------

function validateTelnr  (valfield,   // element to be validated
                         infofield,  // id of element to receive info/error msg
                         required)   // true if required
{
  var stat = commonCheck (valfield, infofield, required);
  if (stat != proceed) return stat;

  var tfld = trim(valfield.value);  // value of field with whitespace trimmed off
  var telnr = /^\+?[0-9 ()-]+[0-9]$/  ;
  if (!telnr.test(tfld)) {
    msg (infofield, "error", "ERROR: not a valid telephone number. Characters permitted are digits, space ()- and leading +");
    setfocus(valfield);
    return false;
  }

  var numdigits = 0;
  for (var j=0; j<tfld.length; j++)
    if (tfld.charAt(j)>='0' && tfld.charAt(j)<='9') numdigits++;

  if (numdigits<6) {
    msg (infofield, "error", "ERROR: " + numdigits + " digits - too short");
    setfocus(valfield);
    return false;
  }

  if (numdigits>14)
    msg (infofield, "warn", numdigits + " digits - check if correct");
  else { 
    if (numdigits<10)
      msg (infofield, "warn", "Only " + numdigits + " digits - check if correct");
    else
      msg (infofield, "warn", "");
  }
  return true;
}

// --------------------------------------------
//             validateAge
// Validate person's age
// Returns true if OK 
// --------------------------------------------

function validateAge    (valfield,   // element to be validated
                         infofield,  // id of element to receive info/error msg
                         required)   // true if required
{
  var stat = commonCheck (valfield, infofield, required);
  if (stat != proceed) return stat;

  var tfld = trim(valfield.value);
  var ageRE = /^[0-9]{1,3}$/
  if (!ageRE.test(tfld)) {
    msg (infofield, "error", "ERROR: not a valid age");
    setfocus(valfield);
    return false;
  }

  if (tfld>=200) {
    msg (infofield, "error", "ERROR: not a valid age");
    setfocus(valfield);
    return false;
  }

  if (tfld>110) msg (infofield, "warn", "Older than 110: check correct");
  else {
    if (tfld<7) msg (infofield, "warn", "Bit young for this, aren't you?");
    else        msg (infofield, "warn", "");
  }
  return true;
}
