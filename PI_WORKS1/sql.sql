
CREATE TABLE INFO_LOG_2 PCTFREE 0 NOLOGGING   AS 
 SELECT  11 AS PLAY_ID,
 1 AS SONG_ID,
 1 CLIENT_ID, trunc(sysdate) AS PLAY_TS
    FROM dual ;
    
	
declare
don number     := 1000;
tarih date :=trunc(sysdate);
BEGIN
execute immediate 'truncate table INFO_LOG_4';
FOR i in 1..don 
loop

tarih := tarih - round(DBMS_RANDOM .value(1,10));

insert into INFO_LOG_4 
SELECT  11 AS PLAY_ID,
 LEVEL AS SONG_ID,
 i CLIENT_ID, tarih AS PLAY_TS
    FROM dual
    connect by level <=i;

insert into INFO_LOG_4 
SELECT  11 AS PLAY_ID,
 LEVEL *10 AS SONG_ID,
 i CLIENT_ID, tarih AS PLAY_TS
    FROM dual
    connect by level <=i;
    
    insert into INFO_LOG_4 
SELECT  11 AS PLAY_ID,
 LEVEL *11 AS SONG_ID,
 i CLIENT_ID, tarih AS PLAY_TS
    FROM dual
    connect by level <=i;

    insert into INFO_LOG_4 
SELECT  11 AS PLAY_ID,
 LEVEL AS SONG_ID,
 i*don CLIENT_ID, tarih AS PLAY_TS
    FROM dual
    connect by level <=i;
    
end loop;
commit;
end ;
	


WITH M_C AS ( 
SELECT COUNT( DISTINCT SONG_ID) AS DISTINCT_PLAY_COUNT , 1 AS CLIENT_N FROM INFO_LOG_3 --WHERE PLAY_TS = '10082016' 
GROUP BY CLIENT_ID )  
 SELECT  DISTINCT_PLAY_COUNT, SUM(CLIENT_N) AS CLIENT_COUNT FROM M_C
  GROUP BY DISTINCT_PLAY_COUNT 
  order by 1  
  

  WITH M_C AS ( 
SELECT COUNT( DISTINCT SONG_ID) AS DISTINCT_PLAY_COUNT , 1  AS CLIENT_N FROM INFO_LOG_3  
GROUP BY CLIENT_ID )  
 SELECT DISTINCT    DISTINCT_PLAY_COUNT, 
SUM( CLIENT_N ) OVER(PARTITION BY DISTINCT_PLAY_COUNT ORDER BY DISTINCT_PLAY_COUNT )   AS CLIENT_COUNT FROM M_C
 ORDER BY 1 
