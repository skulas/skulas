-- Connect this query to the source server

exec msdb.dbo.rds_backup_database 
        @source_db_name='trums', 
        @s3_arn_to_backup_to='arn:aws:s3:::tcstorage1/stage_bu_file_for_copy',
        @overwrite_S3_backup_file=1;
		

exec msdb.dbo.rds_task_status @db_name='trums'


--- Before restoring remember to connect this query to the destination server

exec msdb.dbo.rds_restore_database 
        @restore_db_name='trums', 
        @s3_arn_to_restore_from='arn:aws:s3:::tcstorage1/stage_bu_file_for_copy';

exec msdb.dbo.rds_task_status @db_name='trums'
