-- First adding admin user in terminal.
-- Then:
UPDATE `spotifydb`.`users` SET `roles` = 'admin' WHERE (`userId` = '1');