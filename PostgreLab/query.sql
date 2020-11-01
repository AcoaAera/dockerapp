/*
Create table
*/
CREATE TABLE measurement (
    city_id         int not null,
    logdate         date not null,
    peaktemp        int,
    unitsales       int
) PARTITION BY RANGE (logdate);

/*
Create partitions
*/
CREATE TABLE measurement_y2006m02 PARTITION OF measurement
    FOR VALUES FROM ('2006-02-01') TO ('2006-03-01');

CREATE TABLE measurement_y2006m03 PARTITION OF measurement
    FOR VALUES FROM ('2006-03-01') TO ('2006-04-01');


CREATE TABLE measurement_y2007m11 PARTITION OF measurement
    FOR VALUES FROM ('2007-11-01') TO ('2007-12-01');

CREATE TABLE measurement_y2007m12 PARTITION OF measurement
    FOR VALUES FROM ('2007-12-01') TO ('2008-01-01');

/*
Create index
*/
CREATE INDEX ON measurement_y2006m02 (logdate);
CREATE INDEX ON measurement_y2006m03 (logdate);
CREATE INDEX ON measurement_y2007m11 (logdate);
CREATE INDEX ON measurement_y2007m12 (logdate);

/*
Add some data
*/
INSERT INTO public.measurement(
city_id, logdate, peaktemp, unitsales)
VALUES (1, '2006-02-01', 1, 1);
INSERT INTO public.measurement(
city_id, logdate, peaktemp, unitsales)
VALUES (1, '2006-03-01', 1, 1);
INSERT INTO public.measurement(
city_id, logdate, peaktemp, unitsales)
VALUES (1, '2006-03-01', 1, 1)
