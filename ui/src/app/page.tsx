'use client'

import { useEffect, useState } from "react";

export default function Home() {

  let [definitions, setDefinitions] = useState<any[]>([]);

  useEffect(() => {
    fetch("http://localhost:3001/taskDefinitions")
      .then(res => res.json())
      .then(definitions => setDefinitions(definitions));
  }, []);

  return (
    <main className="">
      <h1>Hello World</h1>
      {
        definitions.map((definition) => <TaskDefinition key={definition.id} definition={definition} />)
      }
    </main>
  );
}

function TaskDefinition(props: any) {
  console.log("Definition: ", props.definition);
  return (
    <div>Name: {props.definition.name}</div>
  )
}