---
name: "devspark.add-application"
description: "Run the DevSpark add-application command"
---

## Prompt Resolution

Determine the current git user by running `git config user.name`. 
Normalize to a folder-safe slug: lowercase, replace spaces with hyphens, strip non-alphanumeric/hyphen chars.

Read and execute the instructions from the **first file that exists**:
1. .documentation/{git-user}/commands/devspark.add-application.md (personalized override)
2. .documentation/commands/devspark.add-application.md (team customization)
3. .devspark/defaults/commands/devspark.add-application.md (stock default)

## User Input

{{input}}

Pass the user input above to the resolved prompt.
