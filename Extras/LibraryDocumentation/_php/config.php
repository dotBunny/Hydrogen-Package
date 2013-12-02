<?php

//
// _php/config.php
//
// Author:
//   Matthew Davey <matthew.davey@dotbunny.com>
//
// Copyright (c) 2013 dotBunny Inc. (http://www.dotbunny.com)
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.



define("DOC_EMPTY", "To be added.");
define("DOC_OVERWRITE", true);
define("SHOW_WARNINGS", true);
define("DEBUG", true);

// Our Paths
define("SOURCE_PATH", "Source/");
define("LOG_PATH", "Logs/");
define("RELEASE_PATH", "Release/");

$allowed_tags = array('see');

$hot_links = array (
	'character controller' => "UnityEngine.CharacterController",
	'rigidbody' => "UnityEngine.Rigidbody",
	'rigidbodies' => "UnityEngine.Rigidbody",
	'MonoBehaviour' => "UnityEngine.MonoBehaviour",
	);
	
$external_links = array ();
	
$remove_links = array (
	'GUI Scripting Guide', 
	'Character animation examples', 
	'Character Controller component'
	);
	
// Link Map
$unity = array();

// Counters
$warnings['Enumeration'] = 0;
$errors['Enumeration'] = 0;
$warnings['Structure'] = 0;
$errors['Structure'] = 0;
$errors['Class'] = 0;
$warnings['Class'] = 0;
$errors['Delegate'] = 0;
$warnings['Delegate'] = 0;
$warnings['Enumeration-member'] = 0;
$errors['Enumeration-member'] = 0;
$warnings['Structure-member'] = 0;
$errors['Structure-member'] = 0;
$errors['Class-member'] = 0;
$warnings['Class-member'] = 0;
$errors['Delegate-member'] = 0;
$warnings['Delegate-member'] = 0;

$counts['Enumeration']['summary'] = 0;
$counts['Enumeration']['remarks'] = 0;
$counts['Enumeration']['total'] = 0;
$counts['Enumeration']['example'] = 0;
$counts['Enumeration']['parameters'] = 0;
$counts['Class']['summary'] = 0;
$counts['Class']['remarks'] = 0;
$counts['Class']['total'] = 0;
$counts['Class']['example'] = 0;
$counts['Class']['parameters'] = 0;
$counts['Delegate']['summary'] = 0;
$counts['Delegate']['remarks'] = 0;
$counts['Delegate']['total'] = 0;
$counts['Delegate']['example'] = 0;
$counts['Delegate']['parameters'] = 0;
$counts['Structure']['summary'] = 0;
$counts['Structure']['remarks'] = 0;
$counts['Structure']['total'] = 0;
$counts['Structure']['example'] = 0;
$counts['Structure']['parameters'] = 0;
$counts['Enumeration-member']['summary'] = 0;
$counts['Enumeration-member']['remarks'] = 0;
$counts['Enumeration-member']['total'] = 0;
$counts['Enumeration-member']['example'] = 0;
$counts['Enumeration-member']['parameters'] = 0;
$counts['Class-member']['summary'] = 0;
$counts['Class-member']['remarks'] = 0;
$counts['Class-member']['total'] = 0;
$counts['Class-member']['example'] = 0;
$counts['Class-member']['parameters'] = 0;
$counts['Delegate-member']['summary'] = 0;
$counts['Delegate-member']['remarks'] = 0;
$counts['Delegate-member']['total'] = 0;
$counts['Delegate-member']['example'] = 0;
$counts['Delegate-member']['parameters'] = 0;
$counts['Structure-member']['summary'] = 0;
$counts['Structure-member']['remarks'] = 0;
$counts['Structure-member']['total'] = 0;
$counts['Structure-member']['example'] = 0;
$counts['Structure-member']['parameters'] = 0;


