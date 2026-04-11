-- Выполните вручную при остановленном API, если не используете dotnet ef database update:
-- psql -f ...

ALTER TABLE "TaskHistories" ADD COLUMN IF NOT EXISTS "ChangedByDisplayName" character varying(255) NULL;
ALTER TABLE "ProjectHistories" ADD COLUMN IF NOT EXISTS "ChangedByDisplayName" character varying(255) NULL;

ALTER TABLE "TaskHistories" DROP CONSTRAINT IF EXISTS "FK_TaskHistories_Users_ChangedById";
ALTER TABLE "TaskHistories" ALTER COLUMN "ChangedById" DROP NOT NULL;
ALTER TABLE "TaskHistories" ADD CONSTRAINT "FK_TaskHistories_Users_ChangedById"
  FOREIGN KEY ("ChangedById") REFERENCES "Users" ("Id") ON DELETE SET NULL;

ALTER TABLE "ProjectHistories" DROP CONSTRAINT IF EXISTS "FK_ProjectHistories_Users_ChangedById";
ALTER TABLE "ProjectHistories" ADD CONSTRAINT "FK_ProjectHistories_Users_ChangedById"
  FOREIGN KEY ("ChangedById") REFERENCES "Users" ("Id") ON DELETE SET NULL;
