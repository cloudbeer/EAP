-- update 2012-4-13

alter table Z01Customer add 
ManageHot int default 0;

update Z01Customer set ManageHot=0;

