<?php 



function process_dir($dir,$recursive = FALSE) {

	
    if (is_dir($dir)) {
      for ($list = array(),$handle = opendir($dir); (FALSE !== ($file = readdir($handle)));) {
        if (($file != '.' && $file != '..') && (file_exists($path = $dir.'/'.$file))) {
          if (is_dir($path) && ($recursive)) {
            $list = array_merge($list, process_dir($path, TRUE));
          } else {
            $entry = array('filename' => $file, 'dirpath' => $dir);

 //---------------------------------------------------------//
 //                     - SECTION 1 -                       //
 //          Actions to be performed on ALL ITEMS           //
 //-----------------    Begin Editable    ------------------//

  $entry['modtime'] = filemtime($path);

 //-----------------     End Editable     ------------------//
            do if (!is_dir($path)) {
 //---------------------------------------------------------//
 //                     - SECTION 2 -                       //
 //         Actions to be performed on FILES ONLY           //
 //-----------------    Begin Editable    ------------------//

  $entry['size'] = filesize($path);
  
  if (strstr(pathinfo($path,PATHINFO_BASENAME),'log')) {
    if (!$entry['handle'] = fopen($path,r)) $entry['handle'] = "FAIL";
  }
  
  $extension = pathinfo($path,PATHINFO_EXTENSION);
  if ( $extension == "cs" || $extension == "shader" || $extension == "js" || $extension == "boo" ) {  
	  
	  
	  $target_start_line = "Permission is hereby granted, free of charge, to any person obtaining a copy of";
	  $target_end_line = "CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

	  
	  $file_array = file($path);
	  
	  for ($x=0; $x <= count($file_array); $x++)
	  {
		  if ( strpos($file_array[$x], $target_start_line) !== FALSE)
		  {
			  $start_position = $x;
		  }
		  
		  if ( strpos($file_array[$x], $target_end_line) !== FALSE)
		  {
			  $end_postion = $x;
		  }
	  }
	  
	  if ( $start_position && $end_postion)
	  {
	  		$total = $end_postion - $start_position;
	  		for ( $y = $start_position; $y <= $end_postion; $y++)
	  		{
		  		unset($file_array[$y]);
	  		}
	  }

	
	  file_put_contents($path, $file_array);
  }
  
  
 //-----------------     End Editable     ------------------//
              break;
            } else {
 //---------------------------------------------------------//
 //                     - SECTION 3 -                       //
 //       Actions to be performed on DIRECTORIES ONLY       //
 //-----------------    Begin Editable    ------------------//

 //-----------------     End Editable     ------------------//
              break;
            } while (FALSE);
            $list[] = $entry;
          }
        }
      }
      closedir($handle);
      return $list;
    } else return FALSE;
  }
   
   

 
$result = process_dir($argv[1],TRUE);

