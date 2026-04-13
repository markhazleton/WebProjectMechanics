---
name: "devspark.pr-review"
description: "Run the DevSpark pr-review command"
---

## Prompt Resolution

Determine the current git user by running `git config user.name`. 
Normalize to a folder-safe slug: lowercase, replace spaces with hyphens, strip non-alphanumeric/hyphen chars.

Read and execute the instructions from the **first file that exists**:
1. .documentation/{git-user}/commands/devspark.pr-review.md (personalized override)
2. .documentation/commands/devspark.pr-review.md (team customization)
3. .devspark/defaults/commands/devspark.pr-review.md (stock default)

## User Input

{{input}}

Pass the user input above to the resolved prompt.
