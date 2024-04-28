-- SQLite
-- select * from clima_history


INSERT INTO last_ac_status (
                               room_id,
                               date,
                               power,
                               mode,
                               temperature
                           )
select                         1,
                               '2024-03-01 08:30',
                               1,
                               1,
                               20
WHERE NOT EXISTS(SELECT 1 FROM last_ac_status);




INSERT INTO clima_history (
                              id,
                              date,
                              room_id,
                              temperature,
                              humidity
                          )

select                        id,
                              date,
                              room_id,
                              temperature,
                              humidity
FROM                    (                
                        select  '1_2024-03-01 08:30' as id,
                                '2024-03-01 08:30' as date,
                                1 as room_id,
                                23 as temperature,
                                30 as humidity
                        union
                        select  '2_2024-03-01 09:00' as id,
                                '2024-03-01 09:00' as date,
                                1 as room_id,
                                22 as temperature,
                                31 as humidity
                        ) as tb1
WHERE NOT EXISTS(SELECT 1 FROM clima_history);