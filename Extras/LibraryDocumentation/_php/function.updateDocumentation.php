<?php

//
// _php/function.updateDocumentation.php
//
// Author:
//   Matthew Davey <matthew.davey@dotbunny.com>
//
// Copyright (c) 2014 dotBunny Inc. (http://www.dotbunny.com)
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

function updateDocumentation($framework_name, $namespace, $type, $name)
{	
	global $errors;
	global $warnings;
	global $counts;
	
	$counts[$name]['total']++;
	
	$parsedData = array();
	
	// Load XML Doc Template
	$objectXML = simplexml_load_file(BASE_PATH . SOURCE_PATH . $framework_name . "/" . $namespace . "/" . $type . ".xml");

	// Check Unity Pathing
	$unity_path = findUnityPath($type);

	if ( !file_exists($unity_path) )
	{
		$errors[$name]++;
		
		file_put_contents(BASE_PATH . LOG_PATH . "updateDocumentation.log", "ERROR: No ". $name . " file  found (" . 
			SCRIPTREFERENCE_PATH . $type . ".html)" . "\n", FILE_APPEND | LOCK_EX);
		
		return;
	}
	
	$parsedData = parseDocumentation($unity_path);
	
  
	if ( DEBUG ) echo "\n\n==========================\n" . strtoupper($name) . ": " .$type . "\n==========================\nSummary: " . $parsedData['summary'] . "\nRemarks: " . $parsedData['remarks'] . "\nParameters\n" . print_r($parsedData['parameters']) . "\nExample: " . $parsedData['example'] . "\nReturns: " . $parsedData['returns'] . "\n";	;	

	if ( empty( $parsedData['summary']) )
	{
		$objectXML->Docs->summary = DOC_EMPTY;
		
		$warnings[$name]++;
		
		file_put_contents(BASE_PATH . LOG_PATH . "updateDocumentation.log", "WARNING: No " . $name . " summary in " . 
					SCRIPTREFERENCE_PATH . $type . ".html" . "\n", FILE_APPEND | LOCK_EX);
	}
	else
	{
		$counts[$name]['summary']++;
		$objectXML->Docs->summary = $parsedData['summary'];
	}
	
	// Add Remarks to XMLDOC
	if ( !empty($parsedData['remarks'] ) )
	{
		$objectXML->Docs->remarks = $parsedData['remarks'];	
		$counts[$name]['remarks']++;	
	}
	
	if ( !empty($parsedData['returns'] ) )
	{
		$objectXML->Docs->returns = $parsedData['returns'];	
		$counts[$name]['returns']++;	
	}
	
	if ( !empty($parsedData['example'] ) )
	{
		$objectXML->Docs->example = $parsedData['example'];	
		$counts[$name]['example']++;	
	}
	
	if ( !empty($parsedData['parameters']))
	{

		foreach($objectXML->Docs->param as $DocParamObject)
		{
			if ( $parsedData["parameters"][(string)$DocParamObject["name"]] != "" && (string)$DocParamObject["hydrogen_parameter_tag"] != "true")
			{
				// Makes a new param (not what we want)
				$newObject = $objectXML->Docs->addChild("param", $parsedData["parameters"][(string)$DocParamObject["name"]]);
				$newObject->addAttribute("name", (string)$DocParamObject["name"]);
				$newObject->addAttribute("hydrogen_parameter_tag", "true");		
					
				// Flag For Removal
				$DocParamObject["name"] = "DELETEME";	
				
				$counts[$name]['parameters']++;			
			}
		}
	}


	// Member Data

	if ( !empty($objectXML->Members) )
	{
		// Foreach Member of class
		foreach ($objectXML->Members->Member as $MemberObject)
		{	
			
			$parsedData = array();
			
			$counts[$name . '-member']['total']++;
			
			$unity_member_path = findUnityMemberPath($type, $MemberObject['MemberName']);
		
			if ( !file_exists($unity_member_path) )
			{
				$errors[$name . '-member']++;
				
				file_put_contents(BASE_PATH . LOG_PATH . "updateDocumentation.log", "ERROR: No " . $name . "-member  file  found (" . 
					SCRIPTREFERENCE_PATH . str_replace("+", ".", $type) . "." . str_replace("+", ".", $MemberObject["MemberName"]) . ".html\n", FILE_APPEND | 						LOCK_EX);
					
				continue;
			}
	
			$parsedData = parseDocumentation($unity_member_path);
			
			
			if ( DEBUG ) 
			{
				echo "\n-----------------\n" . strtoupper($name) . " MEMBER: " . $MemberObject["MemberName"] . "\n-----------------\nSummary: " . $parsedData['summary'] . "\nRemarks: " . $parsedData['remarks'] . "\nParameters\n" . print_r($parsedData['parameters']) . "\nExample: " . $parsedData['example'] . "\nReturns: " . $parsedData['returns'] . "\n";	
			}
			
			if ( empty( $parsedData['summary']) )
			{
				$MemberObject->Docs->summary = DOC_EMPTY;
				
				$warnings[$name . '-member']++;
				
				file_put_contents(BASE_PATH . LOG_PATH . "updateDocumentation.log", "WARNING: No " . $name . "-member summary in " . 
							SCRIPTREFERENCE_PATH . $type . ".html" . "\n", FILE_APPEND | LOCK_EX);
			}
			else
			{
				$counts[$name . '-member']['summary']++;
				$MemberObject->Docs->summary =  $parsedData['summary'];
			}
			
			// Add Remarks to XMLDOC
			if ( !empty($parsedData['remarks'] ) )
			{
				$MemberObject->Docs->remarks =  $parsedData['remarks'];	
				$counts[$name . '-member']['remarks']++;	
			}
			
			if ( !empty($parsedData['returns'] ) )
			{
				$MemberObject->Docs->returns =  $parsedData['returns'];	
				$counts[$name . '-member']['returns']++;	
			}
			
			if ( !empty($parsedData['example'] ) )
			{
				$MemberObject->Docs->example =  $parsedData['example'];	
				$counts[$name . '-member']['example']++;	
			}
			
			if ( !empty($parsedData['parameters']))
			{

				foreach($MemberObject->Docs->param as $DocParamObject)
				{
					if ( $parsedData["parameters"][(string)$DocParamObject["name"]] != "" && (string)$DocParamObject["hydrogen_parameter_tag"] != "true")
					{
						// Makes a new param (not what we want)
						$newObject = $MemberObject->Docs->addChild("param", $parsedData["parameters"][(string)$DocParamObject["name"]]);
						$newObject->addAttribute("name", (string)$DocParamObject["name"]);
						$newObject->addAttribute("hydrogen_parameter_tag", "true");	
	
							
						// Flag For Removal
						$DocParamObject["name"] = "DELETEME";	
						
						$counts[$name . '-member']['parameters']++;			
					}
				}
			}
									
		}
		
	}


	// Save File
	file_put_contents(BASE_PATH . SOURCE_PATH . $framework_name . "/" . $namespace . "/" . $type . ".xml",  trim(cleanUpParameters($objectXML->asXML())));
}