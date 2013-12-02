<?php

//
// _php/function.parseDocumentation.php
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

function parseDocumentation($unity_path)
{
	$documentation_file = @file_get_contents($unity_path, "r"); 
	
	// Make our DOM holder for the documentation
	$documentation_xml = new DOMDocument();
	$documentation_xml->loadHTML($documentation_file);
	
	// Create our XPath finder
	$summary_finder = new DomXPath($documentation_xml);

	// The summary block of text has the class of "first" in Unity documentation
	$summery_nodes = $summary_finder->query("//*[contains(concat(' ', normalize-space(@class), ' '), ' first ')]");	
	
	// Make a holder DOM as we might be assembling a string here - this covers having two "first" but isnt a good method
	//$summary_DOM = new DOMDocument(); 
	
	foreach ($summery_nodes as $node) 
    {
    	// This makes sure we only get the very last Summary (first)
    	$summary_DOM = new DOMDocument();
    	$summary_DOM->appendChild($summary_DOM->importNode($node,true));
    }
 
    // Clean up the summary
    $summary_text = trim(strip_tags($summary_DOM->saveHTML())); 
	

	
	// Handle Subsection Remarks (Pain IN THE Butt!)
	$remarks_finder = new DomXPath($documentation_xml);
	$remarks_elements = $remarks_finder->query("//*[contains(concat(' ', normalize-space(@class), ' '), ' subsection ')]");	
	$found_summary = false;
	$remarks_text = "";
	$returns_text = "";
	
	$params = array();
	foreach ($remarks_elements as $element) 
    {
	    $node_content = "";
	    
    
	    $returns_flag = false;
	   
	    // Assemble Content (with a space!)
	    foreach ($element->childNodes as $node) 
	    { 
	     	if($node->hasAttributes()) 
			{ 
				$attributes = $node->attributes; 
				if(!is_null($attributes)) 
				{ 
					$force_continue = false;
					
					foreach ($attributes as $index=>$attr) 
					{ 
						//echo $attr->name."=\"".$attr->value."\"\n\n";
						
						// Function Signature
						if (  
							($attr->name == "class" && $attr->value == "sigContainer") ||
							($attr->name == "class" && $attr->value == "sigBlockJS") ||
							($attr->name == "class" && $attr->value == "sigBlockBoo") ||
							($attr->name == "class" && $attr->value == "sigBlockCS"))
						{
						
							$force_continue = true;
							break;
						}
						
						// Parameters - We'll handle these a special way later
						if ($attr->name == "class" && $attr->value == "parameters")
						{
							
							foreach ($node->childNodes as $parameter_row) 
							{
								foreach($parameter_row->childNodes as $parameter_cell)
								{
									if ( $parameter_cell->nodeName == "tr" || $parameter_cell->nodeName == "#text" || empty($parameter_cell->nodeValue)) 
									{ 
										
									}
									else 
									{
										$params[] = (string)$parameter_cell->nodeValue;
									}

								}
							}
							$force_continue = true;
							break;
						}
						
						if ($found_summary && $attr->name == "class" && $attr->value == "first")
						{
							$force_continue = true;
							break;
						}
						

						if ( $attr->name == "class" && $attr->value == "codeExampleRaw" )
						{
							$force_continue = true;
							break;
						}
					} 
					if ( $force_continue ) continue;
				} 
			} 
			
			// Titles
			if ( $node->nodeValue == "Description" || $node->nodeValue == "Parameters") continue;
	    
	    	// Check if its a return value
		    if ( !$returns_flag && $node->nodeName == "strong" && $node->nodeValue == "Returns" )
		    {
			    $returns_flag = true;
			    continue;
		    }

		    
		    if ( $node->nodeName == "img" )
		    {
		    	// Don't exactly like doing this, but seems to be the best way to remove stuff without having lots of extra text showing up.
			    break;
		    }
		   
			// Make sure we dont get any code (as best we can)
	    	if ( $node->nodeName != "pre" && !$returns_flag)
	    	{
	    		$node_content .= trim(strip_tags($node->nodeValue)) . " "; 
	    	}
	    	
	    	// Handle Return Content
	    	if ($returns_flag && !empty($node->nodeValue) && (substr_count($node->nodeValue, " ") > 0 ))
		    {
			    $returns_text .= trim(strip_tags($node->nodeValue)) . " ";
		    }
	    }
	    
	    if ( $found_summary && (strlen($node_content) > 0) && !$returns_flag )
	    {
		   $remarks_text .= trim(strip_tags($node_content));
	    }
	    
	    if ( !empty($summary_text) && stristr($node_content, $summary_text))
		{
			$found_summary = true;
		}
	}
	
	// Create our XPath finder
	$note_finder = new DomXPath($documentation_xml);

	// The summary block of text has the class of "first" in Unity documentation
	$note_nodes = $note_finder->query("//*[contains(concat(' ', normalize-space(@class), ' '), ' note ')]");	
	
	// Make a holder DOM as we might be assembling a string here
	$note_DOM = new DOMDocument(); 
	foreach ($note_nodes as $node) 
    {
    	$note_DOM->appendChild($note_DOM->importNode($node,true));
    }
 
    // Add notes to the remarks
    $remarks_text .= " " . trim(strip_tags($note_DOM->saveHTML())); 

    
    $example_text = "";    
    $example_finder = new DomXPath($documentation_xml);
    
    // The summary block of text has the class of "first" in Unity documentation
	$example_nodes = $example_finder->query("//*[contains(concat(' ', normalize-space(@class), ' '), ' codeExampleRaw ')]");	
    
    // Make a holder DOM as we might be assembling a string here
	$example_DOM = new DOMDocument(); 
	foreach ($example_nodes as $node) 
    {
    	$example_DOM->appendChild($example_DOM->importNode($node,true));
    }
    
    // Add example to the remarks
    $example_text = trim(strip_tags(str_replace("\r", "\n", $example_DOM->saveHTML())));     
    
    // Make sure we have a clean return array
    $return_array = array();
    

	// Parameters
	if ( count($params) > 0 )
	{
	    $parsed_params = array();
	    for($y = 0; $y < count($params); $y = $y + 2)
	    {
	    	$parsed_params[scrubText((string)$params[$y])] = scrubText((string)$params[$y + 1]);
	    }
	    
	    $return_array['parameters'] = $parsed_params;
	}
 
    // Sanitize Our Content
	$return_array['remarks'] = scrubText($remarks_text);
	$return_array['summary'] = scrubText($summary_text);
	$return_array['returns'] = scrubText($returns_text);
	$return_array['example'] = scrubText($example_text);
	
	return $return_array;
}