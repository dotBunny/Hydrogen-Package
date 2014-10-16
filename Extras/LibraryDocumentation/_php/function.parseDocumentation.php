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

	// --- First Pass Description ---
    $description_start = strpos($documentation_file, '<div class="subsection"><h2>Description</h2>');
	
	if ( $description_start > 0 ) {
	    
	    // Bump Start Up
	    $description_start += strlen('<div class="subsection"><h2>Description</h2><p>');
	    
	    // Find End
	    $description_end = strpos($documentation_file, '</div>', $description_start);
	    
	    // Capture
	 	$summary_text = substr($documentation_file, $description_start, $description_end - $description_start);
	}
	
	// --- First Pass Remarks ---
	$remarks_start = strpos($documentation_file, '<div class="subsection"><p>', $description_end);
	
	if ( $remarks_start > 0 ) {
		
		// Bump Start Up
	    $remarks_start += strlen('<div class="subsection">');
	    
	     // Find End
	    $remarks_end = strpos($documentation_file, '</div>', $remarks_start);
	    
	    // Capture
	 	$remarks_text = substr($documentation_file, $remarks_start, $remarks_end - $remarks_start);
	 	
	 	$remarks_text = str_replace("<p>", "<para>", $remarks_text);
	 	$remarks_text = str_replace("</p>", "</para>", $remarks_text);
	}
	
	// --- First Pass Example ---
	$example_start = strpos($documentation_file, '<div class="subsection"><pre class="codeExampleCS">');
	if ( $example_start > 0 ) {

		// Bump Start Up
		$example_start += strlen('<div class="subsection"><pre class="codeExampleCS">');
		
		 // Find End
	    $example_end = strpos($documentation_file, '</pre>', $example_start);
	    
	    $example_text = substr($documentation_file, $example_start, $example_end - $example_start);
	}
	
	// Half assed return fix
	if ( substr($summary_text, 0, 7) == "Returns" ) {
		$returns_text = substr($summary_text, 7);
	}
		
	// Handle Parameters
	$parameters_start = strpos($documentation_file, '<div class="subsection"><h2>Parameters</h2>');
	if ( $parameters_start > 0 ) {
		// Bump Start Up
		$parameters_start += strlen('<div class="subsection"><h2>Parameters</h2>');
		
		$parameters_end = strpos($documentation_file, '</div>', $parameters_start);
		
		$parameters_raw = substr($documentation_file, $parameters_start, $parameters_end - $parameters_start);
		
		$parameters_array = array();
		
		$parameters_raw = str_replace('</td>', 'HYDROGEN_SEPERATOR', $parameters_raw);
		
		if ( strpos($parameters_raw, ',') > 0) {
			$parameters_raw = scrubFile($parameters_raw);

			$parameters_explode = explode( "HYDROGEN_SEPERATOR", $parameters_raw);
			
			array_pop($parameters_explode);
			
			$parameter_count = count($parameters_explode);
			
			for ($i = 0; $i < ($parameter_count); $i += 2) {
				$parameters_array[] = array("name" => scrubText($parameters_explode[$i]), "description" => scrubText($parameters_explode[$i+1]));	
			}
		}


					
	}

	// Data Based Returns
	if ( !empty($parameters_array)) {
		$return_array['parameters'] = $parameters_array;		
	} 
	
		
    // Text Based Returns 
    if ( !empty($remarks_text) ) {
		$return_array['remarks'] = scrubText($remarks_text);	    
    }
    
    if (!empty($summary_text) ) {
		$return_array['summary'] = scrubText($summary_text);	    
    }
    
    if (!empty($returns_text ) ) {
		$return_array['returns'] = scrubText($returns_text);	    
    }
    
    if (!empty($example_text) ) {
	   	$return_array['example'] = scrubText($example_text); 
    }

	
	// TODO
	// Maybe parse links to SEE.namescape
	// //source/@path
	
	return $return_array;
}