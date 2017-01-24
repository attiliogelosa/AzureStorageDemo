rem
rem If net use doesn't work returning "The network path was not found", please check the 445 port on your firewall
rem
rem @cmdkey /add:[ACCOUNT_NAME].file.core.windows.net /user:[ACCOUNT_NAME] /pass:[STORAGE_KEY]
rem @net use [DRIVE_LETTER]: \\[ACCOUNT_NAME].file.core.windows.net\[SHARE-NAME] /u:[ACCOUNT_NAME] [STORAGE_KEY]
rem

@cmdkey /add:aegcognizant.file.core.windows.net /user:aegcognizant /pass:cGF7oxbSPRkf8Q107ba8Q86YTeAmbF/qzRWe0Ye+efhAg0Tm0MctKGNeY0EF2wf0C581IKNlpKumIEOChu6/Yw==
@net use T: \\aegcognizant.file.core.windows.net\sharetest /u:aegcognizant cGF7oxbSPRkf8Q107ba8Q86YTeAmbF/qzRWe0Ye+efhAg0Tm0MctKGNeY0EF2wf0C581IKNlpKumIEOChu6/Yw==
