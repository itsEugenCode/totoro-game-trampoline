# Spec Delta

## Purpose

Финальная лесная поляна: 5 огоньков, отскок от спящего духа, пробуждение и тропинка дальше.

## ADDED Requirements

### Requirement: Five golden orbs
Зона MUST содержать ровно **пять** collectible-огоньков, собираемых существующей системой `collectibles`.

#### Scenario: Ground orb without jump
- **WHEN** игрок подходит к огоньку у входа
- **THEN** огонёк собирается без сложного прыжка

#### Scenario: Low root orb with normal jump
- **WHEN** игрок прыгает на низкий корень у входа
- **THEN** он может собрать второй огонёк без `TotoroBounce`

### Requirement: Root platforms readable
Зона MUST содержать корни на слое опоры (`Ground`) разной высоты: низкий у входа и более высокие у дерева/духа.

#### Scenario: Miss high platform safely
- **WHEN** игрок не долетает до высокой платформы после отскока
- **THEN** он приземляется на более низкую безопасную поверхность без потери уже собранных огоньков

### Requirement: Spirit bounce for high orbs
Высокие огоньки (не менее двух) MUST требовать импульса `TotoroBounce` и руления в воздухе; дух MUST NOT просыпаться от промежуточных отскоков.

#### Scenario: Bounce reaches high orb
- **WHEN** игрок входит в bounce surface живота
- **THEN** он получает импульс сильнее обычного прыжка и может достичь высокой платформы с огоньком

### Requirement: No textual tutorial
Зона MUST NOT требовать текстовых инструкций для обучения отскоку.

#### Scenario: Learn by layout
- **WHEN** игрок впервые проходит поляну
- **THEN** расположение огоньков и живота духа достаточно для самостоятельного открытия механики

### Requirement: Awakening on five collected
После сбора пятого огонька MUST один раз запуститься финальная sequence пробуждения без дополнительной кнопки игрока.

#### Scenario: Sequence runs once
- **WHEN** счётчик сессии достигает 5
- **THEN** запускается sequence пробуждения ровно один раз
- **WHEN** sequence уже завершена
- **THEN** повторный вход в триггеры сбора MUST NOT перезапускать sequence

### Requirement: Glowing path remains
После финала MUST оставаться видимая светящаяся тропинка как указатель пути дальше.

#### Scenario: Path visible after finale
- **WHEN** sequence завершена
- **THEN** игрок видит тропинку и может подойти к ней в той же сцене

### Requirement: Reuse existing systems
Реализация MUST использовать существующие движение, `TotoroBounce`, collectibles и камеру; MUST NOT вводить второй bounce или второй счётчик сбора.
