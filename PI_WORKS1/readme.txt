methods name and descriptions are ;

PiProcessHasList() 
This is first method also most optimal, my favorite method. 
read all all input file and generate output result and export it as csv file.
methods doesn't takes any parameter. 

PiProcessHasList(date p)
this methods is same as above methods  except takes date filter.
filter records while reading input file by given parameter date.

PiProcessHasListOptionalParm
this methods also is same above methods except date is optional, it has a default value
that's why not necessary to overload methods. date filter applies internally control..

PiProcessLinq;
this methods is also same above methodolgy except 
linq functions used for process of generate output csv file
this type method easy for reading and maintenance code...

PiProcessList
this generally is same as PiProcessHasList except 
while all above methods use HashList<int> for store SongID
in this method use List<int> collection  for store SongID
this method performs a peace of better some of situation 
such as if clients donesn't have much more distinct songList.
if there is small array items lenear search is well performed 
if song list is goes huge items for each client 
its also better to use HashList 