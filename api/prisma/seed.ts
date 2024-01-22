import { PrismaClient } from '@prisma/client'

const prisma = new PrismaClient();

async function main() {
    await prisma.frequencyType.createMany({
        data: [
            { name: 'Weekly', id: 1 }
        ]
    });



    await prisma.taskStatus.createMany({
        data: [
            { name: 'Upcoming', id: 1 },
            { name: 'Todo', id: 1 },
            { name: 'Complete', id: 1 },
            { name: 'Deleted', id: 1 }
        ]
    });
}