---
trigger: always_on
---

# Google Antigravity Project Rules
> Generated on 2026-01-16 for Repository Wide

## 1. Project Context
- **Role**: .NET Engineer
- **Tech Stack**: .NET Core, C#, Docker
- **Package Manager**: none
- **Environment**: Backend, Docker

## 2. Antigravity Engineering Standards
### Core Principles & Architecture
> **Language Requirement**: All responses, thinking processes, and task lists must be in **English**.

> **Structured Process**: Follow 'Ideation -> Review -> Task Breakdown'.
> **Layered Architecture**: Strictly follow View -> Logic -> Data one-way dependency flow.
> **Git**: Follow Conventional Commits (feat/fix/docs/refactor)
> **Avoid Long Files**: Avoid files > 400 lines. Split functionality.
> **Event-Driven**: Use Pub/Sub for complex interactions to reduce coupling.
> **Progressive Development**: Core flow first (Upload -> List -> Push), then details.
> **State Management**: Avoid Prop Drilling. Clearly define global vs. local state.
> **KISS Principle**: Keep It Simple, Stupid. Prefer the most direct and stable implementation.

## 3. Workflow & Interaction
- **Tone**: Professional, technical, concise (No fluff).
- **Thinking Process**: Use **First Principles**. Explain *why* before *how*.
- **Fixed Command**: Always include `Implementation Plan` and `Task List` in thinking process.

## 4. Code Quality & Design
- **Architecture**: Enforce Logic Splitting & Composition. Avoid files > 400 lines.
- **Components**: Prefer Functional Components + Hooks over Class Components.
- **Comments**: Detailed comments for critical logic (TCP, File IO, Algorithms).
