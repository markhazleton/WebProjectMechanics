<?php
$headers  = 'MIME-Version: 1.0' . "\r\n";
$headers .= 'Content-type: text/html; charset=iso-8859-1' . "\r\n";
$headers .= 'From: ' . $_POST['senderName'] . ' <' . $_POST['senderEmail'] . '>' . "\r\n";//put your website's name in the header, near the email
$to = $_POST['getterEmail'];//where to send the mail
$subject = 'Email from '.$_POST['senderName'].' via '.$_POST['getterTitle'];//the subject of the mail
$message =  nl2br($_POST['senderMessage']);//the message of the mail. The message is parsed in flash, so this variable contains all the fields that you have in your form
if ($_POST['senderMessage'] != "") {//checks if the script is executed by a person, manually
 mail($to, $subject, $message, $headers);
}
//this is it with the PHP script..the rest is in the FLA ;)
?>
