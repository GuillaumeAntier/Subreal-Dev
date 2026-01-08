
CREATE TABLE IF NOT EXISTS "User" (
    id SERIAL PRIMARY KEY,
    mail VARCHAR(255) NOT NULL UNIQUE,
    pseudo VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL 
);

CREATE TABLE IF NOT EXISTS "Level" (
    id SERIAL PRIMARY KEY,
    description VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS "Resolution_time" (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    level_id INTEGER NOT NULL,
    scoreTime TIME NOT NULL,
    scoreDate DATE NOT NULL DEFAULT CURRENT_DATE,
  
    CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES "User"(id) ON DELETE CASCADE,
    CONSTRAINT fk_level FOREIGN KEY (level_id) REFERENCES "Level"(id) ON DELETE CASCADE
);

CREATE INDEX idx_leaderboard_speed ON "Resolution_time"(level_id, scoreTime);